using MacroBoard;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

class myTcpListener
{
    public myTcpListener()
    {
        Thread newThread = new Thread(new ThreadStart(Run));
        newThread.Start();
    }

    public void Run()
    {
        TcpListener server = null;
        try
        {
            // Set the TcpListener on port 13000.
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("172.23.151.204");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while (true)
            {
                Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                NetworkStream stream = client.GetStream();

                foreach (String s in InitMobileData())
                {
                    SendData(s, stream);
                }
                data = null;

                // Get a stream object for reading and writing

                int i;

                // Loop to receive all the data sent by the client.

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    SendData(data, stream);
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

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }


    public void  SendData(String data, NetworkStream stream)
    {
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
        // Send back a response.
        stream.Write(msg, 0, msg.Length);
    }


    public List<string> InitMobileData()
    {
        List<WorkflowView> lw =  Serialization.getWorkFlowsFromJson();
        List<string> lds = new();
        foreach(WorkflowView wf in lw)
        {
            lds.Add(wf.CurrentworkFlow.imagePath);
            lds.Add(wf.CurrentworkFlow.workflowName);
        }

        return lds;
    }
}

