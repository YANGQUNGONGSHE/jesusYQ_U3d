using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using UnityEngine;

public class AccountService : IAccountService
{
    public void RequestVerfiyCode(string phoneNumber, string purpose, Action<bool, string> callback)
    {
        

        var param = new Dictionary<string,string>()
        {
            {"PhoneNumber",phoneNumber },
            {"Purpose",purpose }
        };
        HttpManager.RequestPost(NEWURLPATH.PhoneLoginVerfiycodeSend,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseSendLGVerifyCode>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                callback(false, rs.ResponseStatus.Message);
            }
        },false);

    }

    public void CheckVerfiyCode(string phoneNumber, string purpose, string token, Action<bool, string> callBack)
    {
        

        var param = new Dictionary<string,string>()
        {
            {"PhoneNumber",phoneNumber },
            {"Purpose",purpose },
            {"Token", token}
        };

        HttpManager.RequestPost(NEWURLPATH.PhoneLoginVerfiycodeCheck,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseAuthenticationLGVerifyCode>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callBack != null)
                    callBack(true, string.Empty);
            }
            else
            {
                if (callBack != null)
                    callBack(false, rs.ResponseStatus.Message);
            }
        },false);
    }

    public void RequestLogins(string phoneNumber, string token, Action<bool, Js_ResponseLogins> callback)
    {
       
        var param = new Dictionary<string,string>()
        {
            {"PhoneNumber",phoneNumber },
            {"Token", token}
        };

        HttpManager.RequestPost(NEWURLPATH.PhoneLogin,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseLogins>(response.DataAsText);

            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, rs);
            }
            else
            {
                if (callback != null)
                    callback(false, rs);
            }

        },false);
    }

    public void ReqCancelBindPhone(string phoneNumber, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.CancelBindPhone+ "?PhoneNumber="+ phoneNumber, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void ReqBindPhone(string phoneNumber, string token, Action<bool, string> callback)
    {
        var param = new Dictionary<string,string>()
        {
            {"PhoneNumber",phoneNumber },
            {"Token", token}
        };

        HttpManager.RequestPost(NEWURLPATH.BindPhone,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });

    }

    public void QuerySingleUserInfo(string userId, Action<bool, Js_ResponeSingleUserInfo> callback)
    {
        
        HttpManager.RequestGet(NEWURLPATH.QueryUserById+ "?UserId="+userId, (request, response) =>
        {
            Log.I("请求个人数据:" + response.Data + "  " + response.Message);
            var rs = JsonMapper.ToObject<Js_ResponeSingleUserInfo>(response.DataAsText);

            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, rs);
            }
            else
            {
                if (callback != null)
                    callback(false, rs);
            }

        },false);
    }

    public void QueryUsersByIds(string userIds, string skip, string limit, Action<bool, string, List<User>> callback)
    {
       HttpManager.RequestGet(NEWURLPATH.QueryUsersByIds+ "?userids="+userIds, (request, response) =>
       {
           var rs = JsonMapper.ToObject<Js_ResponeUsersInfo>(response.DataAsText);
           if (rs.ResponseStatus == null)
           {
               if (callback != null)
                   callback(true, string.Empty, rs.Users);
           }
           else
           {
               if (callback != null)
                   callback(false, rs.ResponseStatus.Message, null);
           }
       } );
    }

    public void QueryUsers(string orderby, string descending, string skip, string limit, Action<bool, string, List<User>> callback)
    {
       HttpManager.RequestGet(NEWURLPATH.QueryUsers+ "?orderby="+orderby+ "&descending="+descending+ "&skip="+skip+ "&limit="+limit,
           (request, response) =>
           {
               var rs = JsonMapper.ToObject<Js_ResponeUsersInfo>(response.DataAsText);
               if (rs.ResponseStatus == null)
               {
                   if (callback != null)
                       callback(true, string.Empty, rs.Users);
               }
               else
               {
                   if (callback != null)
                       callback(false, rs.ResponseStatus.Message, null);
               }
           },false,true);
    }

    public void QueryAccountInfo(Action<bool, Js_ResponeAccountInfo> callback)
    {
        
        HttpManager.RequestGet(NEWURLPATH.QueryAccountInfo, (request, response) =>
        {
            Log.I("请求User数据:" + response.DataAsText.ToString() + "  " + response.Data);
            var rs = JsonMapper.ToObject<Js_ResponeAccountInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, rs);
            }
            else
            {
                if (callback != null)
                    callback(false, rs);
            }

        });
     
    }

    public void LoginOut(Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.QuitOut, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeLoginOut>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void RequestUpdateDisplayName(string displayName, Action<bool, string> callback)
    {

        var param = new Dictionary<string, string>()
        {
            {"DisplayName",displayName }
        };

        HttpManager.RequestPut(NEWURLPATH.UpdateAccountDisPlayName, param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeUpdateAccountDisplayName>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void ReqUpdateUserHead(Texture2D texture2D, Action<bool, string> callback)
    {
       HttpManager.UploadHead(NEWURLPATH.UpdateAccountAvatar, texture2D,null, (request, response) =>
       {
           var rs = JsonMapper.ToObject<Js_ResponeUpdateAccountCover>(response.DataAsText);
           if (rs.ResponseStatus == null)
           {
               if (callback != null)
                   callback(true, rs.AvatarUrl);
               Log.I("修改头像返回的地址："+rs.AvatarUrl);
           }
           else
           {
               if (callback != null)
                   callback(false, rs.ResponseStatus.Message);
           }
       });
    }

    public void ReqUpdateSignature(string signature, Action<bool, string> callback)
    {
        var param = new Dictionary<string,string>()
        {
            {"Signature",signature }
        };

        HttpManager.RequestPut(NEWURLPATH.UpdateAccountSignature,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeUpdateAccount>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        } );
    }

    public void ReqUpdateLocal(string country, string state, string city, Action<bool, string> callback)
    {
        var param = new Dictionary<string, string>()
        {
            {"Country",country },
            {"State", state},
            {"City",city }
        };

        HttpManager.RequestPut(NEWURLPATH.UpdateAccountLocal, param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeUpdateAccount>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void ReqUpdateGender(string gender, Action<bool, string> callback)
    {
        var param = new Dictionary<string, string>()
        {
            {"Gender",gender }
        };

        HttpManager.RequestPut(NEWURLPATH.UpdateAccountGender, param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeUpdateAccount>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void ReqUpdateBirthday(string birthDate, Action<bool, string> callback)
    {
        var param = new Dictionary<string, string>()
        {
            {"BirthDate",birthDate }
        };

        HttpManager.RequestPut(NEWURLPATH.UpdateAccountBirthday, param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeUpdateAccount>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void ReqQueryCountries(Action<bool, List<LocalInfo>> callback)
    {

       HttpManager.RequestGet(NEWURLPATH.QueryCountries, (request, response) =>
       {
           var rs = JsonMapper.ToObject<Js_ResponeCountries>(response.DataAsText);

           if (rs.ResponseStatus == null)
           {
               if (callback != null)
                   callback(true, rs.Countries);
           }
           else
           {
               if (callback != null)
                   callback(true, null);
           }
       } );
    }

    public void ReqQueryStates(string countryid, Action<bool, List<LocalInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryStates+ "?countryid="+countryid, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeStates>(response.DataAsText);

            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, rs.States);
            }
            else
            {
                if (callback != null)
                    callback(false, null);
            }
        } );
    }

    public void ReqQueryCities(string stateid, Action<bool, List<LocalInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryCities+ "?stateid="+stateid, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeCities>(response.DataAsText);

            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, rs.Cities);
            }
            else
            {
                if (callback != null)
                    callback(false, null);
            }
        } );
    }

    public void ReqQueryUserReadRecord(string userid, string parenttype, string parentidprefix,string descending, string skip, string limit,
        Action<bool, string, List<ReadRecordInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryReadRecord+ "?userid="+userid+ "&parenttype=" + parenttype+"&parentidprefix=" + parentidprefix + "&descending=" + descending+ "&skip=" + skip+ "&limit=" + limit, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeReadRecord>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty, rs.Views);
            }
            else
            {
                callback(false, rs.ResponseStatus.Message, null);
            }
        });
    }

    public void ReqQueryUserReadRecordCount(string userid, string parenttype, string parentidprefix, Action<bool, string, ReadRecordCount> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryReadRecordCount+ "?userid="+ userid+ "&parenttype="+ parenttype+ "&parentidprefix="+ parentidprefix, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeReadRecordCount>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty, rs.Counts);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message, null);
            }
        } );
    }

    public void ReqQueryReadRecordCountByUsers(string userids, string parenttype, string parentidprefix, string createdsince,
        Action<bool, string, Dictionary<string, ReadRecordCount>> callback)
    {
       HttpManager.RequestGet(NEWURLPATH.QueryReadRecordCountByUsers+ "?userids="+userids+ "&parenttype="+parenttype+ "&parentidprefix="+parentidprefix+ "&createdsince="+createdsince,
           (request, response) =>
           {
               var rs = JsonMapper.ToObject<Js_ResponeReadRecordCountByUsers>(response.DataAsText);
               if (rs.ResponseStatus == null)
               {
                   if(callback!=null)
                   callback(true, string.Empty, rs.UsersCounts);
               }
               else
               {
                   if (callback != null)
                       callback(false, rs.ResponseStatus.Message, null);
               }
           } );
    }

    public void ReqCollectByUser(string userid, string parenttype, string descending, string skip, string limit, Action<bool, string, List<CollectInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryCollectsByUser+ "?userid="+userid+ "&parenttype="+parenttype+ "&descending="+descending+ "&skip="+skip+"&limit="+limit, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeCollectInfos>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty, rs.Bookmarks);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message, null);
            }
        },false);
    }

    public void ReqDeleteCollectRecord(string parentId, Action<bool, string> callback)
    {
        HttpManager.RequestDelete(NEWURLPATH.ReqDeleteCollect+ "?ParentId=" + parentId, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponseComm>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void ReqCreateReport(string parentType, string parentId, string reason, Action<bool, string, AbuseReport> callback)
    {
        var param = new Dictionary<string,string>()
        {
            {"ParentType",parentType },{"ParentId",parentId },{"Reason",reason}
        };
        HttpManager.RequestPost(NEWURLPATH.CreateReport,param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponeReportInfo>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty, rs.AbuseReport);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message, null);
            }
        });

    }

    public void ReqCreateFeedBack(string content, Action<bool, string> callback)
    {
        var param = new Dictionary<string,string>()
        {
            {"Content",content }
        };
        HttpManager.RequestPost(NEWURLPATH.CreateFeedBack, param, (request, response) =>
        {
            var rs = JsonMapper.ToObject<ResponeFeedBack>(response.DataAsText);

            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                    callback(true, string.Empty);
            }
            else
            {
                if (callback != null)
                    callback(false, rs.ResponseStatus.Message);
            }
        });
    }

    public void ReqQueryPersonalRank(string @orderby, string skip, string limit, Action<bool, string, List<PersonalRankInfo>> callback)
    {
       HttpManager.RequestGet(NEWURLPATH.QueryPersonalRank+ "?orderby="+orderby+ "&skip="+skip+ "&limit="+ limit,
           (request, response) =>
           {
               var rs = JsonMapper.ToObject<Js_ResponePersonalRankInfo>(response.DataAsText);

               if (rs.ResponseStatus == null)
               {
                   if (callback != null)
                   {
                       callback(true, string.Empty, rs.UserRanks);
                   }
               }
               else
               {
                   if (callback != null)
                   {
                       callback(false, rs.ResponseStatus.Message, null);
                   }
               }
           });
    }

    public void ReqQueryGroupsRank(string @orderby, string skip, string limit, Action<bool, string, List<GroupRankInfo>> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryGroupsRank + "?orderby=" + orderby + "&skip=" + skip + "&limit=" + limit,
            (request, response) =>
            {
                var rs = JsonMapper.ToObject<Js_ResponeGroupRankInfo>(response.DataAsText);

                if (rs.ResponseStatus == null)
                {
                    if (callback != null)
                    {
                        callback(true, string.Empty, rs.GroupRanks);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(false, rs.ResponseStatus.Message, null);
                    }
                }
            });
    }

    public void ReqQueryPersonalRankById(string userid, Action<bool, string, PersonalRankInfo> callback)
    {
        HttpManager.RequestGet(NEWURLPATH.QueryPersonalRankById+ "?UserId="+ userid, (request, response) =>
        {
            var rs = JsonMapper.ToObject<Js_ResponePersonalRankInfoById>(response.DataAsText);
            if (rs.ResponseStatus == null)
            {
                if (callback != null)
                {
                    callback(true, string.Empty, rs.UserRank);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(false, rs.ResponseStatus.Message, null);
                }
            }
        });
    }

    public void ReqQueryGroupsRankByIds(string groupids, string @orderby, Action<bool, string, List<GroupRankInfo>> callback)
    {
       HttpManager.RequestGet(NEWURLPATH.QueryGroupRankByIds+"?groupids="+groupids+ "&orderby="+orderby,
           (request, response) =>
           {
               var rs = JsonMapper.ToObject<Js_ResponeGroupRankInfo>(response.DataAsText);

               if (rs.ResponseStatus == null)
               {
                   if (callback != null)
                   {
                       callback(true, string.Empty, rs.GroupRanks);
                   }
               }
               else
               {
                   if (callback != null)
                   {
                       callback(false, rs.ResponseStatus.Message, null);
                   }
               }
           });
    }

    public void ReqQueryLastReadRecordData(string bookid, Action<bool, string, ChapterRead> callback)
    {
       HttpManager.RequestGet(NEWURLPATH.ReqQueryLastReadRecord+"?bookid="+bookid, (request, response) =>
       {
           var rs = JsonMapper.ToObject<Js_ResponeLastReadRecordInfo>(response.DataAsText);
           if (rs.ResponseStatus == null)
           {
               if (callback != null)
               {
                   callback(true, string.Empty, rs.ChapterRead);
               }
           }
           else
           {
               if (callback != null)
               {
                   callback(false, rs.ResponseStatus.Message, null);
               }
           }
       });
    }
}