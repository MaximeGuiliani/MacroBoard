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

class myTcpListener
{
    public myTcpListener()
    {
        Thread newThread = new Thread(new ThreadStart(Run));
        newThread.SetApartmentState(ApartmentState.STA);
        newThread.Start();
    }

    public void Run()
    {

        TcpListener server = null;
        try
        {
            // Set the TcpListener on port 13000.
            Int32 port = 13000;
            // IPAddress localAddr = IPAddress.Parse("192.168.43.63");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while (true)
            {
                Trace.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                Trace.WriteLine("Connected!");
                NetworkStream stream = client.GetStream();
                InitMobileData(stream);
                //Console.WriteLine(InitMobileData().Count);


                data = null;

                // Get a stream object for reading and writing

                int i;

                // Loop to receive all the data sent by the client.

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Trace.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    ExecuteGivenWorkflow(data.ToString());
                }

                // Shutdown and end connection
                client.Close();
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }

        Trace.WriteLine("\nHit enter to continue...");
        Console.Read();
    }

    private void ExecuteGivenWorkflow(String Name)
    {

    }

    public void InitMobileData(NetworkStream stream)
    {
        List<WorkflowView> lw = Serialization.getFavsFromJson();
        List<string> lds = new();
        byte[] msg = null;
        foreach (WorkflowView wf in lw)
        {
            // Send back a response.
            lds.Add(wf.CurrentworkFlow.imagePath);
            lds.Add(wf.CurrentworkFlow.workflowName);

            //msg = System.Text.Encoding.ASCII.GetBytes(wf.CurrentworkFlow.workflowName + "\n");

            //Image image = Image.FromFile(wf.CurrentworkFlow.imagePath);


            Bitmap tImage = new Bitmap(wf.CurrentworkFlow.imagePath);

            byte[] test = (System.Text.Encoding.ASCII.GetBytes("<img>"));
            // byte[] bStream = Encoding.ASCII.GetBytes(tImage.ToString());

            byte[] bStream = ImageToByteArray(tImage);
            byte[] test2 = System.Text.Encoding.ASCII.GetBytes("</img>");
            byte[] saut = System.Text.Encoding.ASCII.GetBytes("\n");

            byte[] all = new byte[test.Length + test2.Length + bStream.Length + 2];

            test.CopyTo(all, 0);
            saut.CopyTo(all, test.Length);
            bStream.CopyTo(all, test.Length + 1);
            saut.CopyTo(all, bStream.Length + test.Length + 1);
            test2.CopyTo(all, bStream.Length + test.Length + 2);
            saut.CopyTo(all, test2.Length+ bStream.Length + test.Length + 1);





            stream.Write(all, 0, all.Length);



        }

        msg = System.Text.Encoding.ASCII.GetBytes("|");
        stream.Write(msg, 0, msg.Length);

    }


    public byte[] ImageToByteArray(Image imageIn)
    {
        using (var ms = new MemoryStream())
        {
            imageIn.Save(ms, imageIn.RawFormat);
            return ms.ToArray();
        }
    }
}

