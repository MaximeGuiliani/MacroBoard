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
    public TcpListener server;
    public TcpListener dataReceiveServer;
    public bool isDatasender = true;
    public myTcpListener()
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
            // IPAddress localAddr = IPAddress.Parse("192.168.43.63");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            byte[] bytes = new byte[256];

            // Enter the listening loop.
            while (true)
            {
                Trace.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                Trace.WriteLine("Connected!");
                Thread newThread = new(new ThreadStart(RunDataSender));
                newThread.SetApartmentState(ApartmentState.STA);
                newThread.Start();
                NetworkStream stream = client.GetStream();
                
                InitMobileData(stream);



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
            Trace.WriteLine("\n Server Stopped...");

        }

        Trace.WriteLine("\n Server Closed...");
        
        Console.Read();
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
            TcpClient client = dataReceiveServer.AcceptTcpClient();

            Trace.WriteLine("Connected!");
            NetworkStream streamReceiveMobiledata = client.GetStream();
            GetMobileData(streamReceiveMobiledata);


            // Shutdown and end connection
            client.Close();

        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
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
            // write du client ---------------------------------------------------------------//

            byte[] clientResponseData = new byte[8];
            streamReceiveMobiledata.Read(clientResponseData, 0, clientResponseData.Length);
            string ClientResponse = Encoding.ASCII.GetString(clientResponseData);

            Trace.WriteLine("\n"+"Received from Client : " + ClientResponse+"\n");


            Serialization.ExecuteFromMobileApp(ClientResponse);

            //write du client end ---------------------------------------------------------------//
        }
    }
    private void InitMobileData(NetworkStream stream)
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

