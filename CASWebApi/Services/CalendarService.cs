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
   public class CalendarService //service for creating communication with google calendar 
    {
        
        public static string[] Scopes = { Google.Apis.Calendar.v3.CalendarService.Scope.Calendar };
        public static string ApplicationName = "calendarProject235";
        private static string CredentialsPath = "credentials.json";

        /// <summary>
        ///function to get a list of user's calendars
        /// </summary>
        /// <returns>list of calendars</returns>
        public static IList<CalendarListEntry> GetCalendarList()
        {
            UserCredential credential = GetCredential(UserRole.Admin);
            Google.Apis.Calendar.v3.CalendarService service = GetService(credential);
            var calendars = service.CalendarList.List().Execute().Items;
            
                
            return calendars;
        }


        /// <summary>
        /// create event in given calendar 
        /// </summary>
        /// <param name="newSchedule">schedule object to add</param>
        /// <param name="calendarName">the name of the calendar where we want to add the event</param>
        public static bool CreateEvent(Schedule newSchedule,string calendarName)
        {
            UserCredential credential = GetCredential(UserRole.Admin);
            Google.Apis.Calendar.v3.CalendarService service = GetService(credential);
            var calendars = GetCalendarList();
            string calendarId = getCalendarIdByName(calendarName);
            string reccurence;
            string description=String.Empty;
            string untilDate;
            if(newSchedule.Teacher != null)
                description = String.Format("Teacher: {0} {1}   ID:{2}", newSchedule.Teacher.First_name, newSchedule.Teacher.Last_name, newSchedule.Teacher.Id);
            
            untilDate = newSchedule.LastDate.ToString("yyyyMMdd");
            reccurence = String.Format("RRULE:FREQ=WEEKLY;UNTIL={0}", untilDate);
                Event newEvent = new Event()
                {
                    Summary = newSchedule.Title,
                    Start = new EventDateTime() { DateTime = newSchedule.Start, TimeZone = "Asia/Jerusalem" },
                    End = new EventDateTime() { DateTime = newSchedule.End, TimeZone = "Asia/Jerusalem" },
                    Description=description,
                    Recurrence = new String[] { reccurence }
                };

                if (!(String.IsNullOrEmpty(calendarId)))
                {
                    newEvent = service.Events.Insert(newEvent, calendarId).Execute();
                    newSchedule.EventId = newEvent.Id;
                    
                }
            return newSchedule.EventId != null;
            
        }
        public static Schedule UpdateEvent(Schedule scheduleIn, string calendarName)
        {
            UserCredential credential = GetCredential(UserRole.Admin);
            Google.Apis.Calendar.v3.CalendarService service = GetService(credential);
            var calendars = GetCalendarList();
            string calendarId = getCalendarIdByName(calendarName);
            string reccurence;
            string description = String.Empty;
            string untilDate;
            if (scheduleIn.Teacher != null)
                description = String.Format("Teacher: {0} {1}   ID:{2}", scheduleIn.Teacher.First_name, scheduleIn.Teacher.Last_name, scheduleIn.Teacher.Id);

            untilDate = scheduleIn.LastDate.ToString("yyyyMMdd");
            reccurence = String.Format("RRULE:FREQ=WEEKLY;UNTIL={0}", untilDate);
            Event newEvent = new Event()
            {
                Summary = scheduleIn.Title,
                Start = new EventDateTime() { DateTime = scheduleIn.Start, TimeZone = "Asia/Jerusalem" },
                End = new EventDateTime() { DateTime = scheduleIn.End, TimeZone = "Asia/Jerusalem" },
                Description = description,
                Recurrence = new String[] { reccurence }
            };

            if (!(String.IsNullOrEmpty(calendarId)))
            {
                newEvent = service.Events.Update(newEvent,calendarId,scheduleIn.EventId).Execute();

            }
            return scheduleIn;

        }

        /// <summary>
        /// delete recurring event by eventId
        /// </summary>
        /// <param name="calendarName">name of calendar</param>
        /// <param name="eventId">id of the event to delete</param>
        /// <returns>true if deleted,false otherwise</returns>
        public static bool DeleteEvent(string calendarName,string eventId)
        {
            UserCredential credential = GetCredential(UserRole.Admin);

            Google.Apis.Calendar.v3.CalendarService service = GetService(credential);
            var calendars = GetCalendarList();
            // var events = ShowUpCommingEvent(calendarName);
            string calendarId = getCalendarIdByName(calendarName);
            if (!String.IsNullOrEmpty(calendarId))
                return service.Events.Delete(calendarId, eventId).Execute() != null;
            return false;
            
        }
        

        /// <summary>
        /// function to create new secondary calendar in google calendar
        /// </summary>
        /// <param name="calendarName">name of new calendar to create</param>
        /// <returns>id of created calendar</returns>
        public static string CreateCalendar(string calendarName)
        {
            UserCredential credential = GetCredential(UserRole.Admin);

            Google.Apis.Calendar.v3.CalendarService service = GetService(credential);
            Google.Apis.Calendar.v3.Data.Calendar calendar=new Google.Apis.Calendar.v3.Data.Calendar();
            calendar.Summary = calendarName;
            
            //calendar.Id = calendarName;
           var addedCalendar= service.Calendars.Insert(calendar).Execute();
            return addedCalendar.Id;
            
        }

        /// <summary>
        /// get id of calendar by his name
        /// </summary>
        /// <param name="calendarName">name of calendar to find his id</param>
        /// <returns>id of calendar</returns>
        private static string getCalendarIdByName(string calendarName)
        {
            var calendars = GetCalendarList();
            string calendarId = string.Empty;
            if (calendars != null)
            {
                foreach (var calendar in calendars)
                {
                    if (calendar.Summary.Equals(calendarName))
                    {
                        calendarId = calendar.Id;
                        break;
                    }
                }
            }
            return calendarId;
        }

        /// <summary>
        /// function to delete secondary calendar by name
        /// </summary>
        /// <param name="calendarName">name of calendar to delete</param>
        /// <returns>true if deleted,false otherwise</returns>
        public static bool DeleteCalendar(string calendarName)
        {
            UserCredential credential = GetCredential(UserRole.Admin);

            Google.Apis.Calendar.v3.CalendarService service = GetService(credential);
           
            
            string calendarId =getCalendarIdByName(calendarName);
            if (!(String.IsNullOrEmpty(calendarId)))
            {
                var deletedCalendar = service.Calendars.Delete(calendarId).Execute();
                return deletedCalendar != null;
            }
            return false;

        }
        private static Google.Apis.Calendar.v3.CalendarService GetService(UserCredential credential)
        {
            // Creat Google Calendar API service.
            var service = new Google.Apis.Calendar.v3.CalendarService(new BaseClientService.Initializer()
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
