using System;
using System.ComponentModel.DataAnnotations;

namespace stonks.Models
{
	public class ReportedReplyPost
	{
		/// <summary>
		/// The id of the report
		/// </summary>
		[Key]
		[Required]
		public Guid ReportId { get; set; }

		/// <summary>
		/// The id of the post that was reported
		/// foreign key
		/// </summary>
		[Required]
		public Guid ReplyPostId { get; set; }


		public ReportedReplyPost(Guid reportId, Guid replyPostId)
		{
			ReportId = reportId;
			ReplyPostId = replyPostId;
		}
	}
}
