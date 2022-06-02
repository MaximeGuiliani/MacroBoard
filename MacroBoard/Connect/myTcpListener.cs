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

    public MyTcpListener()
    {
        Thread newThread = new Thread(new ThreadStart(Run));
        newThread.SetApartmentState(ApartmentState.STA);
        newThread.Start();
    }

    public void Run()
    {
        try
        {
            // Set the TcpListener on port 13000.
            Int32 port = 13000;
            server = new TcpListener(port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.

            Trace.Write("Waiting for a connection... ");
            TcpClient client = server.AcceptTcpClient();
            Trace.WriteLine("Connected!");
            NetworkStream stream = client.GetStream();

            InitMobileData(stream);

            client.Close();
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            server.Stop();
        }

        Trace.WriteLine("\nHit enter to continue...");
        Console.Read();
    }

    public void InitMobileData(NetworkStream stream)
    {

        List<WorkflowView> lw = Serialization.getFavsFromJson();
        byte[] msg;
        byte[] clientResponseData;
        string ClientResponse;
        int bytes;
        //foreach (WorkflowView wf in lw)
        //{
        //    //Send Name Start ---------------------------------------------------------------//
        //    byte[] wfNameToSend = Encoding.ASCII.GetBytes(wf.CurrentworkFlow.workflowName);
        //    stream.Write(wfNameToSend, 0, wfNameToSend.Length);
        //    Trace.WriteLine("Name send : " + wf.CurrentworkFlow.workflowName);
        //    // reponse du client
        //    clientResponseData = new byte[256];
        //    bytes = stream.Read(clientResponseData, 0, clientResponseData.Length);
        //    ClientResponse = Encoding.ASCII.GetString(clientResponseData);
        //    Trace.WriteLine("Received from Client : " + ClientResponse);
        //    stream.Flush();
        //    //Send Name end ---------------------------------------------------------------//

        //    //Send image length Start ---------------------------------------------------------------//

        //    Bitmap Image = new Bitmap(wf.CurrentworkFlow.imagePath);
        //    byte[] imageInBytes = ImageToByteArray(Image);
        //    byte[] imageLength = Encoding.ASCII.GetBytes(imageInBytes.Length.ToString());
        //    stream.Write(imageLength, 0, imageLength.Length);
        //    Trace.WriteLine("Image Length send : " + Encoding.ASCII.GetString(imageLength));
        //    // reponse du client
        //    clientResponseData = new byte[256];
        //    bytes = stream.Read(clientResponseData, 0, clientResponseData.Length);
        //    ClientResponse = Encoding.ASCII.GetString(clientResponseData);
        //    Trace.WriteLine("Received from Client : " + ClientResponse);
        //    stream.Flush();
        //    //Send image length Start ---------------------------------------------------------------//

        //    //Send image Start ---------------------------------------------------------------//
        //    byte[] bytesArray = new byte[1024];
        //    Array.Copy(imageInBytes, 0, bytesArray, 0, Math.Min(1024, imageInBytes.Length));
        //    stream.Write(bytesArray, 0, bytesArray.Length);
        //    //Send image end ---------------------------------------------------------------//


        //    //Send balise image end Start ---------------------------------------------------------------//
        //    byte[] balise = Encoding.ASCII.GetBytes("</img>");
        //    stream.Write(balise, 0, balise.Length);
        //    clientResponseData = new Byte[256];
        //    bytes = stream.Read(clientResponseData, 0, clientResponseData.Length);
        //    ClientResponse = Encoding.ASCII.GetString(clientResponseData);
        //    Trace.WriteLine("Received: " + ClientResponse);
        //    stream.Flush();
        //    //Send balise image end Start ---------------------------------------------------------------//
        //}
        ////message to tell the client we finish sending ----------------------------------------//
        //msg = Encoding.ASCII.GetBytes("|");
        //stream.Write(msg, 0, msg.Length);

        Bitmap imageBitmap = new Bitmap(lw[0].CurrentworkFlow.imagePath);
        imageBitmap = resizeImage(imageBitmap, new Size(128, 128));

        byte[] imageInBytes = ImageToByte(imageBitmap);
        byte[] serverResponse = new byte[50];

        stream.Write(Encoding.ASCII.GetBytes(imageInBytes.Length.ToString()), 0, imageInBytes.Length.ToString().Length);

        stream.Read(serverResponse, 0, serverResponse.Length);
        Trace.WriteLine(Encoding.ASCII.GetString(serverResponse));
        
        stream.Write(imageInBytes, 0, imageInBytes.Length);

        serverResponse = new byte[50];
        stream.Read(serverResponse, 0, serverResponse.Length);
        Trace.WriteLine(Encoding.ASCII.GetString(serverResponse));

        stream.Write(Encoding.ASCII.GetBytes(lw[0].CurrentworkFlow.workflowName), 0, lw[0].CurrentworkFlow.workflowName.Length);

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
}

