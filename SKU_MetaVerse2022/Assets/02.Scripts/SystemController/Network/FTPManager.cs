using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;

public class FTPManager : MonoBehaviour
{
    public string UploadFile(string ftpPath, string fileName, string userName, string pwd, string UploadDirectory = "")
    {
        string pureFileName = new FileInfo(fileName).Name;
        string uploadPath = string.Format("{0}{1}/{2}", ftpPath, UploadDirectory, pureFileName);
        FtpWebRequest req = (FtpWebRequest)WebRequest.Create(uploadPath);

        req.Proxy = null;
        req.Method = WebRequestMethods.Ftp.UploadFile;
        req.Credentials = new NetworkCredential(userName, pwd);
        req.UseBinary = true;
        req.UsePassive = true;

        byte[] data = File.ReadAllBytes(fileName);
        req.ContentLength = data.Length;
        Stream stream = req.GetRequestStream();
        stream.Write(data, 0, data.Length);

        stream.Close();

        FtpWebResponse res = (FtpWebResponse)req.GetResponse();
        return res.StatusDescription;
    }

    public string DownloadFile(string ftpPath, string FileNameToDownload, string userName, string password, string tempDirPath)
    {
        string ResponseDescription = "";
        string pureFileName = new FileInfo(FileNameToDownload).Name;
        string DownloadFilePath = tempDirPath + "/" + pureFileName;
        string downloadUrl = string.Format("{0}/{1}", ftpPath, FileNameToDownload);

        FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(downloadUrl);
        req.Method = WebRequestMethods.Ftp.DownloadFile;

        req.Credentials = new NetworkCredential(userName, password);
        req.UseBinary = true;
        req.Proxy = null;

        try
        {
            FtpWebResponse response = (FtpWebResponse)req.GetResponse();
            Stream stream = response.GetResponseStream();
            byte[] buffer = new byte[2048];
            FileStream fs = new FileStream(DownloadFilePath, FileMode.Create);

            int ReadCount = stream.Read(buffer, 0, buffer.Length);
            while (ReadCount > 0)
            {
                fs.Write(buffer, 0, ReadCount);
                ReadCount = stream.Read(buffer, 0, buffer.Length);
            }
            ResponseDescription = response.StatusDescription;
            fs.Close();
            stream.Close();
        }
        catch { }
        return ResponseDescription;
    }
}
