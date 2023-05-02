using System;
using System.Collections.Generic;

public class Datum
{
    public DateTime created_time { get; set; }
    public string message { get; set; }
    public string id { get; set; }
}

public class Posts
{
    public List<Datum> data { get; set; }
}

public class Root
{
    public Posts posts { get; set; }
    public string id { get; set; }
}

public class SocialFeeds
{
    public string WhichPage { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PostedOn { get; set; }
}