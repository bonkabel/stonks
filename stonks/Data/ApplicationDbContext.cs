using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using stonks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace stonks.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostTag>().HasKey(c => new { c.PostId, c.TagId });
            builder.Entity<StockPrice>().HasKey(c => new { c.StockId, c.Date });
            builder.Entity<PeopleTracking>().HasKey(c => new { c.StockId, c.UserId });

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReply> MessageReplies { get; set; }
        public DbSet<PortfolioStock> PortfolioStocks { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReply> PostReplies { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PeopleTracking> PeopleTracking {get; set;}
        public DbSet<ReportedMessage> ReportedMessages { get; set; }
        public DbSet<ReportedMessageReply> ReportedMessageReplies { get; set; }
        public DbSet<ReportedReplyPost> ReportedReplyPosts { get; set; }
        public DbSet<ReportedPost> ReportedPosts { get; set; }
        public DbSet<ReportedUser> ReportedUsers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
