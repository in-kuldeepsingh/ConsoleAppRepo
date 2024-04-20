
namespace ConsoleApp01
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using MetadataExtractor;
    using System.Linq;

    //using Microsoft.WindowsAPICodePack.Shell;

    public static class ImageProcessing
    {

        public static void RenameFilesByDate()
        {
            int i = 0;
            string targetfileName = "";
            DateTime creationDate = DateTime.Parse("Jan 11, 1950");
            var dateList = new List<DateTime>();
            string sourcePath = @"C:\Users\insin\OneDrive\2 Kd_Photos\2010_12 Bharatpur";
            //string sourcePath = Path.Combine("C:", "Users", "insin", "Downloads", "ip01");
            //foreach (string sourceImage in System.IO.Directory.GetFiles("C://Users//HP//Downloads//Norway/", "*.jpg"))

            foreach (var filePath in System.IO.Directory.GetFiles(sourcePath))
            {
                i++;
                var extension = Path.GetExtension(filePath).ToLower();
                var fileName = System.IO.Path.GetFileName(filePath).ToString();
                if (1 == 1)//(fileName.StartsWith('P'))
                {
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                    {
                        creationDate = GetCreatedDateFromMetadata(filePath, "ImageDate");
                    }
                    else if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
                    {
                        creationDate = GetCreatedDateFromMetadata(filePath, "MOVDate");
                        //creationDate = creationDate.AddHours(-8);
                    }
                    else
                    {
                        Console.WriteLine($"Unsupported File: {filePath}"); // Optional handling for other file types
                    }

                    // if no date found in metadata compare with default value returned from function
                    if (creationDate.ToString("ddMMyyyy") == "11011950")
                    {
                        targetfileName = System.IO.Path.GetFileName(filePath).ToString();
                        targetfileName = targetfileName.Remove(0, 4);
                        Console.WriteLine($"{filePath} -{i}-> {targetfileName} << NoDate >>");
                    }
                    // if date found in metadata
                    else
                    {
                        // if file with the same data exists in target, increamenting generated date
                        while (dateList.Contains(creationDate))
                        {
                            creationDate = creationDate.AddSeconds(1);
                            Console.Write($"<< Duplicate Date >>");
                        }
                        dateList.Add(creationDate);

                        targetfileName = $"{creationDate:yyyyMMdd_HHmmss}{extension}";
                        Console.WriteLine($"{filePath} -{i}-> {targetfileName}");
                    }

                    string targetFilePath = Path.Combine("C:", "Users", "insin", "Downloads", "op02", targetfileName);
                    System.IO.File.Copy(filePath, targetFilePath, true);
                }
            }
        }

        public static void RenameImagesByDate()
        {
            int i = 0;
            var dateList = new List<DateTime>();
            string sourcePath = Path.Combine("C:", "Users", "insin", "OneDrive", "2 Kd_Photos", "2019_02 USA");
            //string sourcePath = Path.Combine("C:", "Users", "insin", "Downloads", "ip01");
            //foreach (string sourceImage in System.IO.Directory.GetFiles("C://Users//HP//Downloads//Norway/", "*.jpg"))
            foreach (string sourceImage in System.IO.Directory.GetFiles(sourcePath))
            {
                i++;
                if (sourceImage.EndsWith(".jpg") || sourceImage.EndsWith(".jpeg") || sourceImage.EndsWith(".JPG"))
                {
                    DateTime dt = GetCreatedDateFromMetadata(sourceImage, "ImageDate");
                    //dt = dt.AddHours(2);
                    string targetfileName = "";

                    // if no date found in metadata compare with default value returned from function
                    if (dt.ToString("ddMMyyyy") == "11011950")
                    {
                        targetfileName = System.IO.Path.GetFileName(sourceImage).ToString();
                        //string targetfileName1 = targetfileName.Substring(0, 6);
                        //targetfileName = "20110221_" + targetfileName1 + ".jpg";

                        Console.WriteLine($"{sourceImage} -{i}-> {targetfileName} <NoDate>");
                    }
                    // if date found in metadata
                    else
                    {
                        // if file with the same data exists in target, increamenting generated date
                        while (dateList.Contains(dt))
                        {
                            dt = dt.AddSeconds(1);
                            Console.WriteLine($"Duplicate Date {dt} for {sourceImage}");
                        }
                        dateList.Add(dt);

                        targetfileName = $"{dt:yyyyMMdd_HHmmss}.jpg";
                        Console.WriteLine($"{sourceImage} -{i}-> {targetfileName}");

                    }
                    string filePath = Path.Combine("C:", "Users", "insin", "Downloads", "op02", targetfileName);
                    System.IO.File.Copy(sourceImage, filePath, true);
                }
                else
                {
                    Console.WriteLine($"{sourceImage} -{i}- >>>>>>>>>>>>");
                }
            }
            Console.ReadLine();
        }

        public static void RenameMOVbyDate()
        {
            int i = 0;
            var dateList = new List<DateTime>();
            foreach (string sourceImage in System.IO.Directory.GetFiles("C://Users//insin//Downloads//2//Mov/"))
            {
                i++;
                if (sourceImage.EndsWith(".mov") || sourceImage.EndsWith(".MOV") || sourceImage.EndsWith(".mp4"))
                {
                    DateTime dt = GetCreatedDateFromMetadata(sourceImage, "MOVDate");
                    string targetfileName = "";

                    // if no date found in metadata compare with default value returned from function
                    if (dt.ToString("ddMMyyyy") == "11011950")
                    {
                        targetfileName = System.IO.Path.GetFileName(sourceImage).ToString();
                        targetfileName = targetfileName.Remove(0, 4);
                        Console.WriteLine($"{sourceImage} -{i}-> {targetfileName} <NoDate>");
                    }
                    // if date found in metadata
                    else
                    {
                        // if file with the same data exists in target, increamenting generated date
                        while (dateList.Contains(dt))
                        {
                            dt = dt.AddSeconds(1);
                            Console.WriteLine($"Duplicate Date {dt} for {sourceImage}");
                        }
                        dateList.Add(dt);

                        targetfileName = $"VID_{dt:yyyyMMdd_HHmmss}.MOV";
                        Console.WriteLine($"{sourceImage} -{i}-> {targetfileName}");

                    }
                    string filePath = Path.Combine("C:", "Users", "insin", "Downloads", "op1", targetfileName);
                    System.IO.File.Copy(sourceImage, filePath, true);
                }
                else
                {
                    Console.WriteLine($"{sourceImage} -{i}- >>>>>>>>>>>>");
                }

            }
            Console.ReadLine();
        }

        public static DateTime GetCreatedDateFromMetadata(string filePath, string metadatatype)
        {
            var metaDirectories = MetadataExtractor.ImageMetadataReader.ReadMetadata(filePath);
            //foreach (var directory in metaDirectories)
            //    foreach (var tag in directory.Tags)
            //        Console.WriteLine($"-------{directory.Name} - {tag.Name} = {tag.Description}");
            try
            {
                if (metadatatype == "ImageDate")
                {
                    foreach (var directory in metaDirectories)
                    {
                        foreach (var tag in directory.Tags)
                        {
                            if (tag.Name == "Date/Time Original")
                            {
                                if (string.IsNullOrEmpty(tag.Description))
                                    continue;
                                string d = tag.Description.Split(" ")[0].Replace(":", "-");
                                string t = tag.Description.Split(" ")[1];
                                return DateTime.Parse($"{d} {t}");
                            }
                        }
                    }
                }
                else if (metadatatype == "MOVDate")
                {
                    foreach (var directory in metaDirectories.Where(x => x.Name == "QuickTime Metadata Header"))
                        foreach (var tag in directory.Tags)
                        {
                            if (tag.Name == "Creation Date")
                            {
                                if (string.IsNullOrEmpty(tag.Description))
                                    continue;
                                string d = tag.Description.Split(" ")[0].Replace(":", "-");
                                string dd = tag.Description.Split(" ")[2];
                                string MMM = tag.Description.Split(" ")[1];
                                string yyyy = tag.Description.Split(" ")[5];
                                string t = tag.Description.Split(" ")[3];
                                return DateTime.Parse($"{dd} {MMM} {yyyy} {t}");
                            }
                        }

                    //// temp code for MP4
                    //foreach (var directory in metaDirectories.Where(x => x.Name == "QuickTime Movie Header"))
                    //    foreach (var tag in directory.Tags)
                    //    {
                    //        if (tag.Name == "Created")
                    //        {
                    //            if (string.IsNullOrEmpty(tag.Description))
                    //                continue;
                    //            string d = tag.Description.Split(" ")[0].Replace(":", "-");
                    //            string dd = tag.Description.Split(" ")[2];
                    //            string MMM = tag.Description.Split(" ")[1];
                    //            string yyyy = tag.Description.Split(" ")[4];
                    //            string t = tag.Description.Split(" ")[3];
                    //            return DateTime.Parse($"{dd} {MMM} {yyyy} {t}");
                    //        }
                    //    }
                }
            }
            catch (System.Exception)
            {

                return DateTime.Parse("Jan 11, 1950");
            }
            return DateTime.Parse("Jan 11, 1950");

            //throw new InvalidOperationException($"Date not found in {filePath}");
        }
    }
}
