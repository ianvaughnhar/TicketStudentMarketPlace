using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

//Taylor Campbell
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Demo
{
    public class Function
    {
        private static HttpClient httpClient;

        public Function()
        {
            httpClient = new HttpClient();
        }

        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            string outputText = "";
            var requestType = input.GetRequestType();

            if (requestType == typeof(LaunchRequest))
            {
                return BodyResponse("Welcome to the Student Ticket Marketplace, what would you like to do here!", false);
            }

            else if (requestType == typeof(IntentRequest))
            {
                //outputText += "Request type is Intent";
                var intent = input.Request as IntentRequest;

                if (intent.Intent.Name.Equals("Availability"))
                {
                    var EventName = intent.Intent.Slots["EventName"].Value;
                    var EventDate = intent.Intent.Slots["EventDate"].Value;

                    if (EventName == null)
                    {
                        return BodyResponse("I did not understand the event name of the ticket, please try again.", false);
                    }

                    else if (EventDate == null)
                    {
                        return BodyResponse("I did not understand the event date of the ticket you wanted, please try again.", false);
                    }

                    var TicketInfo = await GetTicketInfo(EventName, EventDate, context);
                    {
                        outputText = $"The price for the{Tickets.EventName} ticket on {Tickets.EventDate.ToString()} is {Tickets.Price}";

                        return BodyResponse(outputText, true);
                    }
                }

                if (intent.Intent.Name.Equals("Sell_Intent"))
                {
                    var EventName = intent.Intent.Slots["EventName"].Value;
                    var EventDate = intent.Intent.Slots["EventDate"].Value;

                    if (EventName == null)
                    {
                        return BodyResponse("I did not understand the event name of the ticket, please try again.", false);
                    }

                    else if (EventDate == null)
                    {
                        return BodyResponse("I did not understand the event date of the ticket you wanted, please try again.", false);
                    }

                    if (intent.Intent.Name.Equals("Price_Intent"))
                    {
                        var EventName = intent.Intent.Slots["EventName"].Value;
                        var EventDate = intent.Intent.Slots["EventDate"].Value;

                        if (EventName == null)
                        {
                            return BodyResponse("I did not understand the event name of the ticket, please try again.", false);
                        }

                        else if (EventDate == null)
                        {
                            return BodyResponse("I did not understand the event date of the ticket you wanted, please try again.", false);
                        }
                        else if (intent.Intent.Name.Equals("AMAZON.StopIntent"))
                        {

                            return BodyResponse("You have now exited the Student Ticket MarketPlace", true);
                        }

                        else
                        {
                            return BodyResponse("I did not understand this intent, please try again", true);
                        }
                    }

                    else
                    {
                        return BodyResponse("I did not understand your request, please try again", true);
                    }


                }

                private SkillResponse BodyResponse(string outputSpeech,
                bool shouldEndSession,
                string repromptText = "Just say, tell me average ticket price.")
                {
                    var response = new ResponseBody
                    {
                        ShouldEndSession = shouldEndSession,
                        OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
                    };

                    if (repromptText != null)
                    {
                        response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromptText } };
                    }

                    var skillResponse = new SkillResponse
                    {
                        Response = response,
                        Version = "1.0"
                    };
                    return skillResponse;
                }


                public async Task<Tickets> GetPlayerInfo(object EventName, object EventDate, ILambdaContext context)
                {
                    Tickets ticket = new Tickets();
                    var uri = new Uri($"http://www.oumisprojects.com/mis3033/api/Marketplace_Table");

                    try
                    {
                        //This is the actual GET request
                        var response = await httpClient.GetStringAsync(uri);
                        context.Logger.LogLine($"Response from URL:\n{response}");
                        // TODO: (PMO) Handle bad requests
                        //Conver the below from the JSON output into a list of player objects
                        ticket = JsonConvert.DeserializeObject<Player>(response);
                    }
                    catch (Exception ex)
                    {
                        context.Logger.LogLine($"\nException: {ex.Message}");
                        context.Logger.LogLine($"\nStack Trace: {ex.StackTrace}");
                    }

                    return ticket;
                }

            }

        }

    }

}



