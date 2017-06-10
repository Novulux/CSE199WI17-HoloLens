using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System;

/* This script is used to sort data from XML files so it can be read with TTS*/
public class rssreader: MonoBehaviour
{
    //XmlTextReader rssReader;
    XmlReader rssReader;
    XmlDocument rssDoc;
    XmlNode nodeRss;
    XmlNode nodeChannel;
    XmlNode nodeItem;
    WWW www;
    string xmlData;
    public channel rowNews;
    System.IO.Stream blah;

    // this is the root channel information within the RSS
    public struct channel
    {
        public string title;
        public string description;
        
        public List<items> item;    //collection of RSS items
    }
   
    //should add the relevant elements as needed
    public struct items
    {
        public string title;
        public string media_description;
        public string description;
    }
    //not used since it doesn't work in Unity right now 
    //maybe could use #if WINDOWS_UWP
    /* private async void getStream(string url)
     {
         HttpClient client = new HttpClient();
         blah =  await client.GetStreamAsync(url);
     }*/

     //this might not work in hololens
    /* public IEnumerator Start()
     {
         www = new WWW("http://twitrss.me/twitter_user_to_rss/?user=realDonaldTrump");
         yield return www;
         xmlData = www.text;
     }*/

    // since I can't get urls to work in hololens, I'll just read from file for now
    //this for news from NYT. Different news sites seem to use different XML formats for the feeds though
    public rssreader()
    {
        rowNews = new channel();
        rowNews.item = new List<items>();
        //load xml data into document
        TextAsset xmlAsset = (TextAsset)Resources.Load("nytworld");
        rssDoc = new XmlDocument();
        rssDoc.LoadXml(xmlAsset.text);
        // Loop for the <rss> tag
        for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
        {
            // If it is the rss tag
            if (rssDoc.ChildNodes[i].Name == "rss")
            {
                // <rss> tag found
                nodeRss = rssDoc.ChildNodes[i];
            }
        }

        for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
        {
            // If it is the channel tag
            if (nodeRss.ChildNodes[i].Name == "channel")
            {
                // <channel> tag found
                nodeChannel = nodeRss.ChildNodes[i];
            }
        }

        //select elements that we find necessary
        rowNews.title = nodeChannel["title"].InnerText;
        //rowNews.link = nodeChannel["link"].InnerText;
        rowNews.description = nodeChannel["description"].InnerText;
        //	rowNews.docs = nodeChannel["docs"].InnerText;
        //	rowNews.lastBuildDate = nodeChannel["lastBuildDate"].InnerText;
        //	rowNews.managingEditor = nodeChannel["managingEditor"].InnerText;
        //	rowNews.webMaster = nodeChannel["webMaster"].InnerText;

        for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
        {
            if (nodeChannel.ChildNodes[i].Name == "item")
            {
                nodeItem = nodeChannel.ChildNodes[i];
                // create an empty item to fill
                items itm = new items();
                itm.title = nodeItem["title"].InnerText;

             //   Debug.Log(itm.title);
                //	itm.link = nodeItem["link"].InnerText;
                //itm.category = nodeItem["category"].InnerText;
                //itm.creator = nodeItem["dc:creator"].InnerText;
                //	itm.guid = nodeItem["guid"].InnerText;
                //	itm.pubDate = nodeItem["pubDate"].InnerText;
                //sometimes there are no media descriptions
                try {   
                if (nodeItem["media:description"] == null) { }
                    itm.media_description = nodeItem["media:description"].InnerText;
                //if ... check for any other nodes you need to
                }catch (Exception e)
                { itm.media_description = "";  } //some node doesn't exists.}
               // Debug.Log(itm.media_description);
                itm.description = nodeItem["description"].InnerText;
                // add the item to the channel items list
                rowNews.item.Add(itm);
            }
        }
    }

    //while this should ideally take a URL, it just reads from file for now
    //lots of methods to read directly from URL with XMLReader worked in Unity but not hololens
    public rssreader(string feedURL)
    {
        rowNews = new channel();
        rowNews.item = new List<items>();
        // rssReader = new XmlTextReader(feedURL);
        /*     
          rssDoc = new XmlDocument();
          rssDoc.Load(blah);*/
        // rssReader = XmlReader.Create(feedURL);
        //    rssDoc = new XmlDocument();
        //     rssDoc.Load(rssReader);

        TextAsset xmlAsset = (TextAsset)Resources.Load("twitter_user_to_rss");
        rssDoc = new XmlDocument();
        rssDoc.LoadXml(xmlAsset.text);

       /* rssDoc = new XmlDocument();
        System.IO.StringReader stringReader = new System.IO.StringReader(xmlData);
        stringReader.Read();
        rssDoc.LoadXml(stringReader.ReadToEnd());*/
        //   GetXml(feedURL);

        // Loop for the <rss> tag
        for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
        {
            // If it is the rss tag
            if (rssDoc.ChildNodes[i].Name == "rss")
            {
                // <rss> tag found
                nodeRss = rssDoc.ChildNodes[i];
            }
        }
        // Loop for the <channel> tag
        for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
        {
            // If it is the channel tag
            if (nodeRss.ChildNodes[i].Name == "channel")
            {
                // <channel> tag found
                nodeChannel = nodeRss.ChildNodes[i];
            }
        }
        // this is our channel header information
        rowNews.title = nodeChannel["title"].InnerText;
        //rowNews.link = nodeChannel["link"].InnerText;
        rowNews.description = nodeChannel["description"].InnerText;
        //	rowNews.docs = nodeChannel["docs"].InnerText;
        //	rowNews.lastBuildDate = nodeChannel["lastBuildDate"].InnerText;
        //	rowNews.managingEditor = nodeChannel["managingEditor"].InnerText;
        //	rowNews.webMaster = nodeChannel["webMaster"].InnerText;

        // here we have our feed items
        for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
        {
            if (nodeChannel.ChildNodes[i].Name == "item")
            {
                nodeItem = nodeChannel.ChildNodes[i];
                // create an empty item to fill
                items itm = new items();
                //parses links and pictures in the tweets 
                int index = nodeItem["title"].InnerText.IndexOf("http");
                if (index > 0)
                {
                    itm.title = nodeItem["title"].InnerText.Substring(0, index);
                   // Debug.Log("Entered");
                }
                else
                {
                    index = 0;
                    index = nodeItem["title"].InnerText.IndexOf("pic.twitter");
                    if (index > 0)
                    {
                        itm.title = nodeItem["title"].InnerText.Substring(0, index);
                    }
                    else
                    {
                        itm.title = nodeItem["title"].InnerText;
                    }
                }
                Debug.Log(itm.title);
                //	itm.link = nodeItem["link"].InnerText;
                //itm.category = nodeItem["category"].InnerText;
                //itm.creator = nodeItem["dc:creator"].InnerText;
                //	itm.guid = nodeItem["guid"].InnerText;
                //	itm.pubDate = nodeItem["pubDate"].InnerText;
                itm.description = nodeItem["description"].InnerText;
                // add the item to the channel items list
                rowNews.item.Add(itm);
            }
        }
    }
}