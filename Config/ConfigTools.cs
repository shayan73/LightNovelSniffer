﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace LightNovelSniffer.Config
{
    internal static class ConfigTools
    {
        private static string XML_NOEUD_RACINE = "Parameters";

        public static void InitConf()
        {
            string path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path + "\\Config.xml");

            if (xmlDoc.DocumentElement == null)
                throw new ApplicationException("Impossible de lire le fichier Config.xml");

            XmlNode ofNode = xmlDoc.DocumentElement.SelectSingleNode("outputFolder");
            XmlNode imNode = xmlDoc.DocumentElement.SelectSingleNode("interactiveMode");
            XmlNode authorNode = xmlDoc.DocumentElement.SelectSingleNode("author");
            XmlNode dctNode = xmlDoc.DocumentElement.SelectSingleNode("defaultChapterTitle");
            XmlNode lnToRetrieveNode = xmlDoc.DocumentElement.SelectSingleNode("lnToRetrieve");

            if (ofNode != null)
                Globale.OUTPUT_FOLDER = ofNode.InnerText;

            if (imNode != null)
            {
                if (!bool.TryParse(imNode.InnerText, out Globale.INTERACTIVE_MODE))
                    Globale.INTERACTIVE_MODE = true;
            }

            if (authorNode != null)
                Globale.AUTHOR = authorNode.InnerText;

            if (dctNode != null)
                Globale.DEFAULT_CHAPTER_TITLE = dctNode.InnerText;

            Globale.LN_TO_RETRIEVE = new List<LnParameters>();

            if (lnToRetrieveNode != null)
            {
                XmlNodeList lnNodes = lnToRetrieveNode.SelectNodes("ln");
                if (lnNodes != null)
                    foreach (XmlNode ln in lnNodes)
                    {
                        LnParameters lnParameter = new LnParameters();

                        XmlNode nameNode = ln.SelectSingleNode("name");
                        XmlNode coverNode = ln.SelectSingleNode("urlCover");

                        if (nameNode != null)
                            lnParameter.name = nameNode.InnerText;
                        if (coverNode != null)
                            lnParameter.urlCover = coverNode.InnerText;

                        XmlNode versionsNode = ln.SelectSingleNode("versions");
                        if (versionsNode != null)
                        {
                            foreach (XmlNode version in versionsNode.SelectNodes("version"))
                            {
                                UrlParameter up = new UrlParameter();

                                XmlNode urlNode = version.SelectSingleNode("url");
                                XmlNode languageNode = version.SelectSingleNode("language");
                                XmlNode fcNode = version.SelectSingleNode("firstChapterNumber");
                                XmlNode lcNode = version.SelectSingleNode("lastChapterNumber");

                                if (urlNode != null)
                                    up.url = urlNode.InnerText;

                                if (languageNode != null)
                                    up.language = languageNode.InnerText;

                                if (fcNode != null)
                                {
                                    if (!int.TryParse(fcNode.InnerText, out up.firstChapterNumber))
                                        up.firstChapterNumber = 1;
                                }

                                if (lcNode != null)
                                {
                                    if (!int.TryParse(lcNode.InnerText, out up.lastChapterNumber))
                                        up.lastChapterNumber = -1;
                                }

                                lnParameter.urlParameters.Add(up);
                            }
                        }

                        Globale.LN_TO_RETRIEVE.Add(lnParameter);
                    }
            }
        }
    }
}