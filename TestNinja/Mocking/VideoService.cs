using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TestNinja.Mocking
{
    public class VideoService
    {
        public string ReadVideoTitle()
        {
            var str = File.ReadAllText("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
            return video == null ? "Error parsing the video." : video.Title;
        }

        public string GetUnprocessedVideosAsCsv()
        {
            using var context = new VideoContext();
            var videos = 
                (from video in context.Videos
                    where !video.IsProcessed
                    select video).ToList();

            var videoIds = videos.Select(v => v.Id).ToList();

            return string.Join(",", videoIds);
        }
    }

    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }

    public class VideoContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
    }
}