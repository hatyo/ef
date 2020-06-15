using System;
using System.Linq;

namespace EFGetStarted
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var db = new BloggingContext())
            {
                // remove everything to make program runs idempotent
                foreach(var b in db.Blogs)
                {
                    db.Remove(b);
                }
                db.SaveChanges();

                Console.WriteLine("Let us add some posts");

                var b1 = new BloggingContext.Blog { Url = "http://myspace/article1.html" };
                b1.Posts.Add(new BloggingContext.Post { Title = "title1", Content = "Laoreet urna semper ut morbi enim ad" });
                b1.Posts.Add(new BloggingContext.Post { Title = "title2", Content = "Lacus luctus ultrices faucibus facilisis est non donec" });
                db.Add(b1);

                var b2 = new BloggingContext.Blog { Url = "http://myspace/article1.html" };
                b2.Posts.Add(new BloggingContext.Post { Title = "title1", Content = "Tempus suspendisse mattis elit varius" });
                b2.Posts.Add(new BloggingContext.Post { Title = "title2", Content = "Rutrum venenatis dictum arcu aenean" });
                b2.Posts.Add(new BloggingContext.Post { Title = "title3", Content = "Platea donec aenean non venenatis aenean arcu hendrerit aenean etiam" });
                db.Add(b2);

                // write to db
                db.SaveChanges();

                Console.WriteLine("Let's query a post containing word 'mattis'");

                var q = from b in db.Blogs
                        from p in b.Posts
                        where p.Content.Contains("mattis") select p;
                
                Console.WriteLine("Post with title: '" + q.First().Title + "' contains the word 'mattis'");

                // let us now modify that post
                q.First().Title = "modified title";
                db.SaveChanges();

                Console.WriteLine("Let's now print all posts");

                var all_posts = from b in db.Blogs
                         from p in b.Posts
                         select p;

                foreach (var post in all_posts) {
                    Console.WriteLine("post title: '" + post.Title + "', content: " + post.Content);
                }
            }
        }
    }
}
