using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dapr.Cqrs.UI.ViewModels {
    public class ReadSearchPageViewModel {
        public ReadSearchPageViewModel () {
            CurrentPlantKey = "PC";
            CurrentLocationKey = "UFF";
            CurrentTagKey = "TMP";
            ResetCurrentPrefix();
        }
        public string ErrorMessage { get; set; }
        public string CurrentPlantKey { get; set; }
        public string CurrentLocationKey { get; set; }
        public string CurrentTagKey { get; set; }
        public string CurrentPrefix { get; set; }
        public void ResetCurrentPrefix()=>CurrentPrefix = DateTime.Now.ToString("yyyy/MM/dd");
        public bool CanShowDetails => "2021/09/18/0420".Length == CurrentPrefix?.Length;
        private string Convert (string value) {
            var tokens = value.Split ("/").ToList ();
            if (value.EndsWith (".json")) {
                tokens.RemoveAt (tokens.Count - 1);
                return string.Join ("/", tokens.ToArray ());
            }
            return value;
        }
        public IEnumerable<BlobItem> GetReadyToDisplay () {

            foreach (var group in DirectoriesOrFiles.Where (_ => _.Split ("/").Length == 8).GroupBy (_ => Convert (_))) {
                var files = group.Where (_ => _.EndsWith (".json")).Select (_ => Path.GetFileNameWithoutExtension (_));
                var nameTokens = group.Key.Substring (11).Trim ('/').Split ('/');
                var label = $"{nameTokens[3].Substring(0,2)}:{nameTokens[3].Substring(2)}";
                var name = string.Join ("/", nameTokens);
                yield return new BlobItem (name, label, files.Select(_=> _.Split('-').LastOrDefault() ).ToArray ());
            }
        }
        public List<String> DirectoriesOrFiles { get; set; } = new ();
    }
    public record BlobItem (string Name, string Label, string[] Files);
}