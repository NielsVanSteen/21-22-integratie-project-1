using UI.MVC.Extensions;

namespace UI.MVC.Models.Android;

public class StatisticAndroidDto
{
    /// <author>Bjorn Straetemans</author>
    /// <summary>
    /// Who has written the  <see cref="DocReview"/>.
    /// </summary>
    
    public Dictionary<string,string> Statistics { get; set; }

    public StatisticAndroidDto(Domain.ProjectStatistics.ProjectStatistics projectStatistics)
    {
        Statistics = new Dictionary<string, string>();
        Statistics.Add("Comments and sub comments",projectStatistics.ReactionGroupAmount.FormatNumber());
        Statistics.Add("Managers",projectStatistics.ManagersAmount.FormatNumber());
        Statistics.Add("Users",projectStatistics.UsersAmount.FormatNumber());
        Statistics.Add("Emojis",projectStatistics.EmojiAmount.FormatNumber());
        
        foreach (var status in projectStatistics.CommentStatusTypeAmount)
        {
            Statistics.Add(status.CommentStatus.ToString(),status.Total.FormatNumber());
        } 
        
        
    }
}
