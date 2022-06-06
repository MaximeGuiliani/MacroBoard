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

        List<WorkflowView> lwf = Serialization.getFavsFromJson();

        stream.Write(Encoding.ASCII.GetBytes(lwf.Count.ToString()), 0, 1);

        foreach (WorkflowView wf in lwf)
        {
            Bitmap imageBitmap = new Bitmap(wf.CurrentworkFlow.imagePath);
            imageBitmap = resizeImage(imageBitmap, new Size(128, 128));

            byte[] imageInBytes = ImageToByte(imageBitmap);
            byte[] serverResponse = new byte[50];

            stream.Write(Encoding.ASCII.GetBytes(imageInBytes.Length.ToString()), 0, imageInBytes.Length.ToString().Length);

            Trace.WriteLine(imageInBytes.Length);

            stream.Read(serverResponse, 0, serverResponse.Length);
            Trace.WriteLine(Encoding.ASCII.GetString(serverResponse));

            stream.Write(imageInBytes, 0, imageInBytes.Length);

            serverResponse = new byte[50];
            stream.Read(serverResponse, 0, serverResponse.Length);
            Trace.WriteLine(Encoding.ASCII.GetString(serverResponse));

            stream.Write(Encoding.ASCII.GetBytes(wf.CurrentworkFlow.workflowName), 0, wf.CurrentworkFlow.workflowName.Length);

            serverResponse = new byte[50];
            stream.Read(serverResponse, 0, serverResponse.Length);
            Trace.WriteLine(Encoding.ASCII.GetString(serverResponse));
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
}

