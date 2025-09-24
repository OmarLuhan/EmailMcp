using System.ComponentModel;
using EmailMcp.Dto;
using EmailMcp.Notificator;
using ModelContextProtocol.Server;

namespace EmailMcp.Tools;
[McpServerToolType]
public class EmailTool(ISendEmail sendEmail)
{
    [McpServerTool , Description("Send an email, the expected parameters are: to (the email address)," +
                                 " subject (the title or subject of the email), body (the body or" +
                                 " message you want to transmit)")]
    public async Task<string?> SendEmail(string to, string subject, string body)
    {
        try
        {
            var message = new EmailDto
            {
                To = to,
                Subject = subject,
                Body = body
            };
            await sendEmail.Execute(message);
            return "success";
        }
        catch(Exception ex) 
        {
           return ex.Message;
        }
    }

    // [McpServerTool]
    // public Task<string?> SendEmail(string to, string subject, string body, string cc, string bcc)
    // {
    //     throw  new NotImplementedException();
    // }
    [McpServerTool,Description("find the email by sending the name")]
    public static async Task<string> SearchEmails(string name)
    {
        try
        {
            //todo: pass this path through settings or environment variables
            const string pathContacts = "/home/mike/dev/mcps/EmailMcp/EmailMcp/contacts.csv";
            string[] lines= await File.ReadAllLinesAsync(pathContacts);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(',');
                if (parts.Length != 2) continue;
                string contactName= parts[0];
                string email = parts[1];
                if (contactName.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    return $"{contactName} -> {email}";
                }
            }
            return "is not on the contact list";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    [McpServerTool, Description("Add a new email contact with your name and email address.")]
    public static async Task<string> AddContactEmail(string name, string email)
    {
        try
        {
            //todo: pass this path through settings or environment variables
            const string pathContacts = "/home/mike/dev/mcps/EmailMcp/EmailMcp/contacts.csv";
            string[] lines = File.Exists(pathContacts)
                ? await File.ReadAllLinesAsync(pathContacts)
                : [];
            
            if ((from line in lines where !string.IsNullOrWhiteSpace(line) select line.Split(',') into parts where parts.Length == 2 select parts[1].Trim()).Any(existingEmail => existingEmail.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                return $"The email '{email}' is already in the contact list.";
            }
            var newLine = $"{name},{email}{Environment.NewLine}";
            await File.AppendAllTextAsync(pathContacts, newLine);
            return $"Contact added: {name} -> {email}";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

}
