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


                data = null;

                // Get a stream object for reading and writing

                int i;



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



    public void InitMobileData(NetworkStream stream)
    {

        List<WorkflowView> lw = Serialization.getFavsFromJson();
        byte[] msg;
        byte[] clientResponseData;
        string ClientResponse;
        int bytes;
        foreach (WorkflowView wf in lw)
        {
            //Send Name Start ---------------------------------------------------------------//
            byte[] wfNameToSend = Encoding.ASCII.GetBytes(wf.CurrentworkFlow.workflowName);
            stream.Write(wfNameToSend, 0, wfNameToSend.Length);
            Trace.WriteLine("Name send : " + wf.CurrentworkFlow.workflowName);
            // reponse du client
            clientResponseData = new byte[256];
            bytes = stream.Read(clientResponseData, 0, clientResponseData.Length);
            ClientResponse = Encoding.ASCII.GetString(clientResponseData);
            Trace.WriteLine("Received from Client : " + ClientResponse);
            stream.Flush();
            //Send Name end ---------------------------------------------------------------//

            //Send image length Start ---------------------------------------------------------------//
            Bitmap Image = new Bitmap(wf.CurrentworkFlow.imagePath);
            byte[] imageInBytes = ImageToByteArray(Image);
            byte[] imageLength = Encoding.ASCII.GetBytes(imageInBytes.Length.ToString());
            stream.Write(imageLength, 0, imageLength.Length);
            Trace.WriteLine("Image Length send : " + Encoding.ASCII.GetString(imageLength));
            // reponse du client
            clientResponseData = new byte[256];
            bytes = stream.Read(clientResponseData, 0, clientResponseData.Length);
            ClientResponse = Encoding.ASCII.GetString(clientResponseData);
            Trace.WriteLine("Received from Client : " + ClientResponse);
            stream.Flush();
            //Send image length Start ---------------------------------------------------------------//

            //Send image Start ---------------------------------------------------------------//

            stream.Write(imageInBytes, 0, imageInBytes.Length);
            //Send image end ---------------------------------------------------------------//


            //Send balise image end Start ---------------------------------------------------------------//
            byte[] balise = Encoding.ASCII.GetBytes("</img>");
            stream.Write(balise, 0, balise.Length);
            clientResponseData = new Byte[256];
            bytes = stream.Read(clientResponseData, 0, clientResponseData.Length);
            ClientResponse = Encoding.ASCII.GetString(clientResponseData);
            Trace.WriteLine("Received: " + ClientResponse);
            stream.Flush();
            //Send balise image end Start ---------------------------------------------------------------//
        }
        //message to tell the client we finish sending ----------------------------------------//
        msg = Encoding.ASCII.GetBytes("|");
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

