using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;

namespace SimplePhotoGallery.Models
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhotoId { get; set; }

        [Required]
        public string Title { get; set; }

        public string FileName { get; set; }

        public byte[] FileData { get; set; }

        public string ImageMimeType { get; set; }

        [Required]
        public string Description { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Comment> RelatedComments { get; set; }
    }

    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [ForeignKey("RelatedPhoto")]
        public int PhotoId { get; set; }

        public string UserName { get; set; }

        public string Body { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Photo RelatedPhoto { get; set; }
    }
    
    public class CommentsViewModel
    {
        public List<Comment> Comments { get; set; }
        public bool IsAuthenticated { get; set; }
    }

    public class PhotoDbContext : DbContext
    {
        public PhotoDbContext() : base("PhotoDbConnection")
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

    public class PhotoDbInitializer : DropCreateDatabaseAlways<PhotoDbContext>
    {
        protected override void Seed(PhotoDbContext context)
        {
            base.Seed(context);

            List<Photo> photos = new List<Photo>
            {
                new Photo
                {
                    Title = "Blackberries",
                    FileName = "blackberries.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\blackberries.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of blackberries.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Photo
                {
                    Title = "Coastalview",
                    FileName = "coastalview.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\coastalview.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of coastal view.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Photo
                {
                    Title = "Flower",
                    FileName = "flower.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\flower.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of flower.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Photo
                {
                    Title = "Fungi",
                    FileName = "fungi.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\fungi.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of fungi.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Photo
                {
                    Title = "Headland",
                    FileName = "headland.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\headland.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of headland.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Photo
                {
                    Title = "Orchard",
                    FileName = "orchard.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\orchard.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of orchard.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Photo
                {
                    Title = "Path",
                    FileName = "path.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\path.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of path.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new Photo
                {
                    Title = "Ripples",
                    FileName = "ripples.JPG",
                    FileData = GetPhotoFileContent("Content\\SamplePhotos\\ripples.JPG"),
                    ImageMimeType = "image/jpeg",
                    Description = "This is a photo of ripples.",
                    UserName = "user001",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                }
            };
            photos.ForEach(p => context.Photos.Add(p));
            context.SaveChanges();

            List<Comment> comments = new List<Comment>
            {
                new Comment
                {
                    PhotoId = 1,
                    UserName = "user002",
                    Body = "The blackberries show up very clearly.",
                    CreatedDate = DateTime.Now
                },
                new Comment
                {
                    PhotoId = 1,
                    UserName = "user002",
                    Body = "It would be nice to have more colorful background.",
                    CreatedDate = DateTime.Now
                }
            };
            comments.ForEach(c => context.Comments.Add(c));
            context.SaveChanges();
        }

        private byte[] GetPhotoFileContent(string strPath)
        {
            FileStream fs = new FileStream(HttpRuntime.AppDomainAppPath + strPath, FileMode.Open, FileAccess.Read);
            byte[] fileBytes;
            using (BinaryReader br = new BinaryReader(fs))
            {
                fileBytes = br.ReadBytes((int)fs.Length);
            }
            return fileBytes;
        }
    }
}