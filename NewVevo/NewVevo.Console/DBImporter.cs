using NewVevo.Entity;
using NewVevo.Entity.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;

namespace NewVevo.Console
{
    public class DBImporter
    {
        public static void ReadFile(string fileName)
        {
            var allLines = File.ReadAllLines(fileName);

            var videos = new List<Video>();

            foreach (var line in allLines)
            {
                var fields = line.Split('\t');

                var vid = new Video();
                vid.Title = fields[0];
                vid.Isrc = fields[1];
                vid.ArtistId = fields[2];
                vid.Artist = fields[3];

                if (!IsNull(fields[4]))
                    vid.VideoYear = int.Parse(fields[4]);
                if (!IsNull(fields[5]))
                    vid.Duration = TimeSpan.Parse(fields[5]);
                if (!IsNull(fields[6]))
                    vid.TotalViews = long.Parse(fields[6]);
                if (!IsNull(fields[7]))
                    vid.Genre = fields[7];

                videos.Add(vid);
            }

            var byIsrc = videos.GroupBy(v => v.Isrc).Select(v => Combine(v)).ToArray();


            var ctx = new VevoContext();
            ctx.Database.ExecuteSqlCommand("Delete From Videos");

            var currentSkip = 0;
            
            while (currentSkip < byIsrc.Length)
            {
                BatchInsert(byIsrc.Skip(currentSkip).Take(1000), ctx);
                currentSkip += 1000;
            }
        }

        private static void BatchInsert(IEnumerable<Video> byIsrc, VevoContext ctx)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("insert into Videos (title, isrc, artistid, artist, videoyear, duration, totalviews, genre) ");
            foreach (var v in byIsrc)
            {
                sb.AppendFormat("SELECT '{0}','{1}','{2}','{3}',{4},'{5}',{6},'{7}' UNION ",
                    SafeReplace(v.Title),
                    SafeReplace(v.Isrc),
                    SafeReplace(v.ArtistId),
                    SafeReplace(v.Artist),
                    v.VideoYear, v.Duration, v.TotalViews, SafeReplace(v.Genre)
                    );
            }
            sb.Length = sb.Length - 7;

            ctx.Database.ExecuteSqlCommand(sb.ToString());
        }

        private static string SafeReplace(string val)
        {
            if (string.IsNullOrEmpty(val)) return string.Empty;

            return val.Replace("'", "''");
        }

        //private DateTime CoalseceDateTime(DateTime dt)
        //{
        //    if(dt < SqlDateTime.MinValue)
        //        return (DateTime)SqlDateTime.MinValue;

        //    if(dt > SqlDateTime.MaxValue)
        //        return (DateTime)SqlDateTime.MaxValue;

        //    return dt;
        //}

        private static bool IsNull(string p)
        {
            return string.IsNullOrEmpty(p) || p.ToUpper() == "NULL";
        }

        private static Video Combine(IEnumerable<Video> videos)
        {
            var v = videos.First();

            if (videos.Count() == 1) return v;

            foreach (var vid in videos.Skip(1))
            {
                v.Genre += "|" + vid.Genre;
            }

            return v;
        }
    }
}
