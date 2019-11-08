﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TwitchLib.PubSub.Enums;

namespace TwitchLib.PubSub.Models.Responses.Messages
{
    /// <summary>
    /// Leaderboard model constructor.
    /// Implements the <see cref="MessageData" />
    /// </summary>
    /// <seealso cref="MessageData" />
    /// <inheritdoc />
    public class LeaderboardEvents : MessageData
    {
        /// <summary>
        /// Leader board type
        /// </summary>
        /// <value>The type</value>
        public LeaderBoardType Type { get; private set; }
        /// <summary>
        /// Channel id
        /// </summary>
        /// <value>The channel id</value>
        public int ChannelId { get; private set; }
        /// <summary>
        /// Top 10 list of the leaderboards
        /// </summary>
        /// <value>The list of the leaderboard</value>
        public List<Tuple<int, int, int>> Top { get; private set; }

        /// <summary>
        /// LeaderboardEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public LeaderboardEvents(string jsonStr)
        {
            JToken json = JObject.Parse(jsonStr);
            switch (json.SelectToken("domain").First.ToString())
            {
                case "bits-usage-by-channel-v1":
                    Type = LeaderBoardType.bitsUsageByChannel;
                    break;
                case "sub-gift-sent":
                    Type = LeaderBoardType.subGiftSent;
                    break;
            }

            switch (Type)
            {
                case LeaderBoardType.bitsUsageByChannel:
                    ChannelId = int.Parse(json.SelectToken("grouping_key").First.ToString());
                    foreach (var TopBits in json["top"])
                    {
                        Top.Add(new Tuple<int, int, int>(
                            int.Parse(TopBits["rank"].ToString()),
                            int.Parse(TopBits["score"].ToString()),
                            int.Parse(TopBits["entry_key"].ToString())
                            ));
                    }
                    break;
                case LeaderBoardType.subGiftSent:
                    ChannelId = int.Parse(json.SelectToken("grouping_key").First.ToString());
                    foreach (var TopSubs in json["top"])
                    {
                        Top.Add(new Tuple<int, int, int>(
                            int.Parse(TopSubs["rank"].ToString()),
                            int.Parse(TopSubs["score"].ToString()),
                            int.Parse(TopSubs["entry_key"].ToString())
                        ));
                    }
                    break;
            }
        }
    }
}