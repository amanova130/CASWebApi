using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace CASWebApi.Models
{
   public class Calendar
    {
        
        public static string[] Scopes = { CalendarService.Scope.Calendar };
        public static string ApplicationName = "calendarTest2";

        private static string CredentialsPath = "credentials.json";

       /* public TimeTable(string credentialsPath)
        {
            CredentialsPath = credentialsPath;
        }*/

        public static Events ShowUpCommingEvent(string calendarName)
        {
            UserCredential credential = GetCredential(UserRole.User);
            string calendarId = string.Empty;
            // Creat Google Calendar API service.
            CalendarService service = GetService(credential);
            var calendars = GetCalendarList();
            foreach (var calendar in calendars) {
                if (calendarName.Equals(calendar.Summary))
                {
                    calendarId = calendar.Id;
                    break;
                }
                    }

                EventsResource.ListRequest request = service.Events.List(calendarId);
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
               // request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            
                // List events.
                
                Events events = request.Execute();

                // Print upcomming events
                Console.WriteLine("Upcomming events:");
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        string when = eventItem.Start.DateTime.ToString();
                        if (String.IsNullOrEmpty(when))
                        {
                            when = eventItem.Start.Date;
                        }
                        Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                    }
                }
                else
                {
                    Console.WriteLine("Nothing.");
                }

                return events;
            
        }
        public static IList<CalendarListEntry> GetCalendarList()
        {
            UserCredential credential = GetCredential(UserRole.Admin);
            CalendarService service = GetService(credential);
            var calendars = service.CalendarList.List().Execute().Items;
            
                
            return calendars;
        }

        public static void CreateEvent(string calendarName,Schedule[] groupSchedule)
        {
            UserCredential credential = GetCredential(UserRole.Admin);
            CalendarService service = GetService(credential);
            var calendars = GetCalendarList();
            string calendarId=string.Empty;
            bool isCalendarExists = false;
            
            foreach (var calendar in calendars)
            {
                if(calendarName.Equals(calendar.Summary))
                {
                    calendarId = calendar.Id;
                    isCalendarExists = true;
                    break;
                    

                }
            }
            if (!isCalendarExists)
                calendarId=CreateCalendar(calendarName);

            for (int i = 0; i < groupSchedule.Length; i++)
            {
                Event newEvent = new Event()
                {
                    Summary = groupSchedule[i].Summary,
                    Start = new EventDateTime() { DateTime = groupSchedule[i].StartTime, TimeZone = "Asia/Jerusalem" },
                    End = new EventDateTime() { DateTime = groupSchedule[i].EndTime, TimeZone = "Asia/Jerusalem" },
                    Recurrence = new String[] { "RRULE:FREQ=WEEKLY;UNTIL=20210801" }
                };

                // string calendarId = "9ertkf33gp54bfrtc27e31ua34@group.calendar.google.com";
                if (!(String.IsNullOrEmpty(calendarId)))
                {
                    newEvent = service.Events.Insert(newEvent, calendarId).Execute();
                    groupSchedule[i].eventId = newEvent.Id;

                    Console.WriteLine($"{newEvent.HtmlLink}");
                }
            }
        }
        public static void DeleteEvent(string calendarName,string eventName)
        {
            UserCredential credential = GetCredential(UserRole.Admin);

            CalendarService service = GetService(credential);
            var calendars = GetCalendarList();
            var events = ShowUpCommingEvent(calendarName);
            string calendarId=string.Empty;
            if(calendars != null)
            {
                foreach(var calendar in calendars)
                {
                    if(calendar.Summary.Equals(calendarName))
                    {
                        calendarId = calendar.Id;
                    }
                }
            }
            if (events != null && !String.IsNullOrEmpty(calendarId))
            {
                foreach (var eventItem in events.Items)
                {
                    if (eventItem.Summary.Equals(eventName))
                    {
                        service.Events.Delete(calendarId, eventItem.Id).Execute();
                    }
                }
            }
            
        }
        public static string CreateCalendar(string calendarName)
        {
            UserCredential credential = GetCredential(UserRole.Admin);

            CalendarService service = GetService(credential);
            Google.Apis.Calendar.v3.Data.Calendar calendar=new Google.Apis.Calendar.v3.Data.Calendar();
            calendar.Summary = calendarName;
            
            //calendar.Id = calendarName;
           var addedCalendar= service.Calendars.Insert(calendar).Execute();
            return addedCalendar.Id;
            
        }
        private static CalendarService GetService(UserCredential credential)
        {
            // Creat Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        private static UserCredential GetCredential(UserRole userRole)
        {
            UserCredential credential;
            using (var stream =
                new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                userRole.ToString(),
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;

                Console.WriteLine($"Credential file saved to: {credPath}");
            }

            return credential;
        }
    }
}
