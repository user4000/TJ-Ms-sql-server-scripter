using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectStandard
{
  public static class TTConstArray128
  {
    public static int Length { get; } = 128;

    public static string[] Array { get; } =
    {
      "1HvXjx9RCxca8UPAvSYXaar2eLrwi6SCdJVKIcJikv0D6slGGMZcrtw6ksHCIHwNAN1g/X+gTR4V6JnJcIh1/MLAd07pq5Bnkc/7fwuKyesvVm+OvfD/r9xQtMbxAMHnc/LgLIfi7oISIpg/3mMUelbtOZy96FbGIPUhWZqlBho=",
      "rlbMfM7uleg+OotYExotVVisLaGo+hO/M+e2AM96PSUpSNzn3VSnIZp6ZFCrhjID8QS0r2qYNGUJtag0dtNUZaaNyj7Mb3qAhu4Ji2UBXblW56RPkeoCJ3m/wzGlPnjgayMB4FBSLlGYUggvJU4q3/epgbdRAx+5s/fdRw/TBOw=",
      "BFVn5eILQ/qYiaGTTSIiPyeqlM1GayYIW3NDZNTi3n6j5YAi5A7W+UX37F6vAuhX/QhJxtosTePrGd9Wse+5/tmQXL0zWgW9gTDAwbmAJpzKaHQzzDkx7KmvwzHXPu2theer//D8lbG1b1i45zjQ11g9SuZSOcVRhCgfyN14oi8=",
      "qlt2Z+3pwtblCMPbOHJdD7IwiGUlgiV7J/7p2XRu4I42M9AK0nj4qPvjeBGaCYO6AiLoF+wMhaNdT9DYX1+6afIm1mRdModU8Ygm341hYhcVrUyD7xCy5N1SRh9Ct/eHHRVxNb5qt4Whhi7k2XR+vw7LK2lz9RNHHAmHysgmUSI=",
      "v3ZVDF2CwxajXfn6VepYSyvGw5h5QRSf5EIttOnIoUj0WofSWm4k5gyu5kaqBSpOzs8XoqIAQW0RX11G0qZObqXwmq43FwGW68R+mdhfW3OPaDHa+UagDLJOFSWlSIanQtlZqIDJ9fQRzefx3gdHudBGzsVCfAW1hs65H/qy6T0="
    };
      
    public static List<byte[]> ListArray { get; private set; }

    private static void InitListArray()
    {
      ListArray = new List<byte[]>();   byte[] AA;  
      for (int i=0; i < Array.Length; i++)
      {
        AA = Convert.FromBase64String(Array[i]);  ListArray.Add(AA);
      }    
    }
    
    /*
    https://stackoverflow.com/questions/1437352/when-is-a-static-constructor-called-in-c
    When is a static constructor called in C#?
    When the class is accessed for the first time.
    A static constructor is used to initialize any static data, or to perform a particular action that needs performed once only. 
    It is called automatically before the first instance is created or any static members are referenced. 
    */
    static TTConstArray128() => InitListArray();

  }
}

