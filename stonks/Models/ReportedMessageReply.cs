using System;
using System.ComponentModel.DataAnnotations;

namespace stonks.Models
{
	public class ReportedMessageReply
	{
		/// <summary>
		/// The id of the report
		/// </summary>
		[Key]
		[Required]
		public Guid ReportId { get; set; }

		/// <summary>
		/// The id of the reply that was reported
		/// foreign key
		/// </summary>
		[Required]
		public Guid ReplyId { get; set; }


		public ReportedMessageReply(Guid reportId, Guid replyId)
		{
			ReportId = reportId;
			ReplyId = replyId;
		}
	}
}
