using COMPASS.Core;
using COMPASS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace COMPASS.Tools
{
  public static class Utils
  {
    public static int GetAvailableID<T>(IEnumerable<T> collection) where T : IHasID
    {
      var creator = new FreshIdCreator();
      return creator.Create(collection);
    }

    //put all nodes of a tree in a flat enumerable
    public static IEnumerable<T> FlattenTree<T>(IEnumerable<T> l) where T : IHasChildren<T>
    {
      var flattener = new TreeFlattener();
      return flattener.FlattenTree(l);
    }

    //check internet connection
    public static bool PingURL(string URL = "8.8.8.8")
    {
      Ping p = new();
      try
      {
        PingReply reply = p.Send(URL, 3000);
        if (reply.Status == IPStatus.Success)
        {
          return true;
        }
      }
      catch (Exception ex)
      {
        Logger.log.Warn(ex.InnerException);
      }
      return false;
    }

    //Try functions in order determined by list of preferablefunctions untill one succeeds
    public static bool TryFunctions<T>(List<PreferableFunction<T>> toTry, T arg)
    {
      bool success = false;
      int i = 0;
      while (!success && i < toTry.Count)
      {
        success = toTry[i].Function(arg);
        i++;
      }
      return success;
    }
    public static bool TryFunctions<T>(ObservableCollection<PreferableFunction<T>> toTry, T arg)
    {
      return TryFunctions<T>(toTry.ToList(), arg);
    }

    //Download data and put it in a byte[]
    public static async Task<byte[]> DownloadFileAsync(string uri)
    {
      using HttpClient client = new();

      return !Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult)
    ? throw new InvalidOperationException("URI is invalid.")
    : await client.GetByteArrayAsync(uri);
    }

    public static async Task<JObject> GetJsonAsync(string uri)
    {
      using HttpClient client = new();

      JObject json = null;

      if (!Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult))
      {
        throw new InvalidOperationException("URI is invalid.");
      }

      HttpResponseMessage response = await client.GetAsync(uri);
      if (response.IsSuccessStatusCode)
      {
        Task<string> data = response.Content.ReadAsStringAsync();
        json = JObject.Parse(data.Result);
      }
      return json;
    }

    //helper function for InitWebdriver to check if certain browsers are installed
    public static bool IsInstalled(string name)
    {
      string currentUserRegistryPathPattern = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\";
      string localMachineRegistryPathPattern = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\";

      object currentUserPath = Microsoft.Win32.Registry.GetValue(currentUserRegistryPathPattern + name, "", null);
      object localMachinePath = Microsoft.Win32.Registry.GetValue(localMachineRegistryPathPattern + name, "", null);

      return currentUserPath != null | localMachinePath != null;
    }

    public static string FindFileDirectory(string fileName)
    {
      return FindFileDirectory(fileName, Directory.GetCurrentDirectory());
    }
    public static string FindFileDirectory(string fileName, string rootDirectory)
    {
      string filePath = Directory.GetFiles(rootDirectory, fileName, SearchOption.AllDirectories)[0];
      string parentDirectory = Path.GetDirectoryName(filePath);
      return parentDirectory;
    }

  }
}
