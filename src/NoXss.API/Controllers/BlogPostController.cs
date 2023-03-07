using Microsoft.AspNetCore.Mvc;

namespace NoXss.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogPostController : ControllerBase
    {
        private static readonly List<BlogPost> _blogPosts = new()
        {
            new BlogPost
            {
                Id = 1,
                Title = "Test Post 1",
                Body = "The Super Bowl is finally here as the AFC Champion Kansas City Chiefs square off against the NFC Champion Philadelphia Eagles. The Eagles are a slight favorite in the game, which features over 2,000 props from Caesars Sportsbook. Whether it's the side, total or a squares game or prop BINGO, we have you covered. Here is everything you need to know about how to bet the game, our favorite plays and more."
            },
            new BlogPost
            {
                Id = 2,
                Title = "Test Post 2",
                Body = "I have not wagered on the side or the total. It feels un-American, and I hate myself for it. Even providing a pick for ESPN.com and various outlets on Radio Row throughout Super Bowl week in Arizona felt arduous because I take it so seriously. I went with the Eagles 27-17, but I have never had less conviction for a championship game. Simply put, it's difficult to handicap when both quarterbacks are not fully healthy. Additionally, this stylistic matchup lends itself to giant unknowns."
            }
        };

        private readonly ILogger<BlogPostController> _logger;

        public BlogPostController(ILogger<BlogPostController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<BlogPost> Get()
        {
            return _blogPosts;
        }

        [HttpPost]
        public BlogPost Post(BlogPost blogPost)
        {
            blogPost.Id = _blogPosts.Count + 1;

            _blogPosts.Add(blogPost);

            return blogPost;
        }
    }
}