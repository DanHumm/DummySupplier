using System.Text;
using System.Text.Json;
using Logistics_Supplier1_API.Models;

namespace Logistics_Supplier1_API.Helpers;

public class JsonValidator
{
    public static bool IsValidJson(string json)
    {
        try
        {
            JsonSerializer.Deserialize<object>(json);
            return true;
        }
        catch (Exception e)
        {   
            return false;
        }
    }
}