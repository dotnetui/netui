using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo
{
    public static class ScopeExtensions
    {
        public static List<string> ToStringArray(Scopes scopes)
        {
            var list = new List<string>(10);
            list.Add("public");
            if (scopes.HasFlag(Scopes.Private))
                list.Add("private");
            if (scopes.HasFlag(Scopes.Purchased))
                list.Add("purchased");
            if (scopes.HasFlag(Scopes.Create))
                list.Add("create");
            if (scopes.HasFlag(Scopes.Edit))
                list.Add("edit");
            if (scopes.HasFlag(Scopes.Delete))
                list.Add("delete");
            if (scopes.HasFlag(Scopes.Interact))
                list.Add("interact");
            if (scopes.HasFlag(Scopes.Stats))
                list.Add("stats");
            if (scopes.HasFlag(Scopes.Upload))
                list.Add("upload");
            if (scopes.HasFlag(Scopes.PromoCodes))
                list.Add("promo_codes");
            if (scopes.HasFlag(Scopes.VideoFiles))
                list.Add("video_files");
            return list;
        }

        public static Scopes ToScopes(this IEnumerable<string> scopes)
        {
            var result = Scopes.Public;
            foreach (var scope in scopes)
            {
                switch (scope)
                {
                    case "private":
                        result |= Scopes.Private;
                        break;
                    case "purchased":
                        result |= Scopes.Purchased;
                        break;
                    case "create":
                        result |= Scopes.Create;
                        break;
                    case "edit":
                        result |= Scopes.Edit;
                        break;
                    case "delete":
                        result |= Scopes.Delete;
                        break;
                    case "interact":
                        result |= Scopes.Interact;
                        break;
                    case "stats":
                        result |= Scopes.Stats;
                        break;
                    case "upload":
                        result |= Scopes.Upload;
                        break;
                    case "promo_codes":
                        result |= Scopes.PromoCodes;
                        break;
                    case "video_files":
                        result |= Scopes.VideoFiles;
                        break;
                }
            }
            return result;
        }
    }
}
