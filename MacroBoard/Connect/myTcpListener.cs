using MacroBoard;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

class MyTcpListener
{

    public TcpListener server;
    public Thread mainThread;
    public NetworkStream stream;
    public TcpClient client;

    public TcpListener dataReceiveServer;
    public TcpClient clientSender;
    public bool isclientSenderOnline = false;
    public bool isclientOnline = false;
    public bool isDatasender = true;
    public MyTcpListener()

    {
        Thread mainThread = new(Run);
        mainThread.SetApartmentState(ApartmentState.STA);
        mainThread.Start();

    }

    public void Run()
    {

        while (true)
        {
            try
            {
                // Set the TcpListener on port 13000.
                server = new TcpListener(13000);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                byte[] bytes = new byte[256];

                Trace.Write("Waiting for a connection... ");
                client = server.AcceptTcpClient();
                Trace.WriteLine("Connected!");

                isclientOnline = true;
                Thread newThread = new(new ThreadStart(RunDataSender));
                newThread.SetApartmentState(ApartmentState.STA);
                newThread.Start();

                stream = client.GetStream();
                if (stream == null) continue;

                while (true)
                {
                    try
                    {
                        byte[] clientResponse = new byte[50];

                        stream.Read(clientResponse, 0, clientResponse.Length);
                        stream.Write(clientResponse, 0, clientResponse.Length);
                        if (Encoding.ASCII.GetString(clientResponse).Contains("Quit")) break;
                        InitMobileData(stream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }

                }
                if (client != null)
                    client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();

                Trace.WriteLine("\n Server Stopped...");
            }

            Trace.WriteLine("\n Server sending fav Closed...");

            Console.Read();
            isclientOnline = false;
        }



    }


    private void InitMobileData(NetworkStream stream)
    {
        try
        {
            List<WorkflowView> lwf = Serialization.getFavsFromJson();

            stream.Write(Encoding.ASCII.GetBytes(lwf.Count.ToString()), 0, 1);

            foreach (WorkflowView wf in lwf)
            {
                Bitmap imageBitmap;

                if (wf.CurrentworkFlow.imagePath.Length <= 0)
                    imageBitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\Button_Workflow.png");
                else
                    imageBitmap = new Bitmap(wf.CurrentworkFlow.imagePath);


                imageBitmap = resizeImage(imageBitmap, new Size(128, 128));

                byte[] imageInBytes = ImageToByte(imageBitmap);
                byte[] clientResponse = new byte[50];

                stream.Write(Encoding.ASCII.GetBytes(imageInBytes.Length.ToString()), 0, imageInBytes.Length.ToString().Length);

                Trace.WriteLine(imageInBytes.Length);

                stream.Read(clientResponse, 0, clientResponse.Length);
                Trace.WriteLine(Encoding.ASCII.GetString(clientResponse));

                stream.Write(imageInBytes, 0, imageInBytes.Length);

                clientResponse = new byte[50];
                stream.Read(clientResponse, 0, clientResponse.Length);
                Trace.WriteLine(Encoding.ASCII.GetString(clientResponse));

                stream.Write(Encoding.ASCII.GetBytes(wf.CurrentworkFlow.workflowName.Length.ToString()), 0, wf.CurrentworkFlow.workflowName.Length.ToString().Length);

                stream.Write(Encoding.ASCII.GetBytes(wf.CurrentworkFlow.workflowName), 0, wf.CurrentworkFlow.workflowName.Length);

                clientResponse = new byte[50];
                stream.Read(clientResponse, 0, clientResponse.Length);
                Trace.WriteLine(Encoding.ASCII.GetString(clientResponse));
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Exception {0} | {1}", e.Message, "On Run Data Sender");
            server.Stop();
            stream.Close();
        }


    }

    public static byte[] ImageToByte(Image img)
    {
        ImageConverter converter = new ImageConverter();
        return (byte[])converter.ConvertTo(img, typeof(byte[]));
    }

    private static Bitmap resizeImage(Bitmap imgToResize, Size size)
    {
        //Get the image current width  
        int sourceWidth = imgToResize.Width;
        //Get the image current height  
        int sourceHeight = imgToResize.Height;
        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;
        //Calulate  width with new desired size  
        nPercentW = ((float)size.Width / (float)sourceWidth);
        //Calculate height with new desired size  
        nPercentH = ((float)size.Height / (float)sourceHeight);
        if (nPercentH < nPercentW)
            nPercent = nPercentH;
        else
            nPercent = nPercentW;
        //New Width  
        int destWidth = (int)(sourceWidth * nPercent);
        //New Height  
        int destHeight = (int)(sourceHeight * nPercent);
        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage((System.Drawing.Image)b);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        // Draw image with new width and height  
        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();
        return b;
    }




    public void RunDataSender()
    {
        isDatasender = false;

        try
        {
            int port = 14000;

            dataReceiveServer = new TcpListener(port);

            // Start listening for client requests.
            dataReceiveServer.Start();

            // Buffer for reading data
            byte[] bytes = new byte[256];


            Trace.WriteLine("Waiting for a connection Data Sender... ");

            // Perform a blocking call to accept requests.
            // You could also use server.AcceptSocket() here.
            clientSender = dataReceiveServer.AcceptTcpClient();

            Trace.WriteLine("Connected!");
            isclientSenderOnline = true;
            NetworkStream streamReceiveMobiledata = clientSender.GetStream();
            GetMobileData(streamReceiveMobiledata);


            // Shutdown and end connection
            isclientSenderOnline = false;
            clientSender.Close();

        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0} | {1}", e, "On Run Data Sender");
        }
        finally
        {
            // Stop listening for new clients.
            dataReceiveServer.Stop();
            Trace.WriteLine("\n Server Stopped...");

        }

        Console.Read();
    }

    private void GetMobileData(NetworkStream streamReceiveMobiledata)
    {
        while (true)
        {
            try
            {
                // write du client ---------------------------------------------------------------//
                byte[] clientNameSize = new byte[20];
                streamReceiveMobiledata.Read(clientNameSize, 0, clientNameSize.Length);
                string ClientResponseNameSize = Encoding.ASCII.GetString(clientNameSize);


                Trace.WriteLine("\n" + "Received from Client : " + ClientResponseNameSize + "\n");

                byte[] okSend = Encoding.ASCII.GetBytes("ok");
                streamReceiveMobiledata.Write(okSend, 0, okSend.Length);

                byte[] clientResponseData = new byte[Int16.Parse(ClientResponseNameSize)];
                streamReceiveMobiledata.Read(clientResponseData, 0, clientResponseData.Length);
                string ClientResponse = Encoding.ASCII.GetString(clientResponseData);


                Trace.WriteLine("\n" + "Received from Client : " + ClientResponse + "\n");



                Serialization.ExecuteFromMobileApp(ClientResponse);
                Trace.WriteLine("");
                //write du client end ---------------------------------------------------------------//
            }
            catch (Exception)
            {

                Trace.WriteLine("Client was Closed");
                break;
            }

        }
    }



}

