using Newtonsoft.Json;

namespace TrimmerPro.ViewModels;

public class MainShortenerViewModel : BindableBase
{
    private readonly string _opizoApiKey;
    private readonly string _plinkApiKey;
    private readonly string _bitlyApiKey;
    private readonly string _bitlyLoginKey;
    private readonly string _cuttlyApiKey;
    private readonly string _rebrandlyApiKey;
    private readonly string _makhlasApiKey;


    public MainShortenerViewModel()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("TrimmerPro.app-data.txt");
        using var textStreamReader = new StreamReader(stream!);

        _opizoApiKey = textStreamReader.ReadLine()?.Trim() ?? string.Empty;
        _plinkApiKey = textStreamReader.ReadLine()?.Trim() ?? string.Empty;
        _bitlyApiKey = textStreamReader.ReadLine()?.Trim() ?? string.Empty;
        _bitlyLoginKey = textStreamReader.ReadLine()?.Trim() ?? string.Empty;
        _cuttlyApiKey = textStreamReader.ReadLine()?.Trim() ?? string.Empty;
        _rebrandlyApiKey = textStreamReader.ReadLine()?.Trim() ?? string.Empty;
        _makhlasApiKey = textStreamReader.ReadLine()?.Trim() ?? string.Empty;

        _data = new ObservableCollection<ComboModel>();
        _shorterList = new ObservableCollection<ShorterListModel>();
        _isButtonEnable = false;
        _selectedCustomText = string.Empty;
        _shortedUrl = string.Empty;


        Data.Add(new ComboModel {Name = "Opizo", IsCustomTextAvailable = false});
        Data.Add(new ComboModel {Name = "Bitly", IsCustomTextAvailable = false});
        Data.Add(new ComboModel {Name = "PLink", IsCustomTextAvailable = true});
        Data.Add(new ComboModel {Name = "Cuttly", IsCustomTextAvailable = true});
        Data.Add(new ComboModel {Name = "Rebrandly", IsCustomTextAvailable = true});
        Data.Add(new ComboModel {Name = "TinyUrl [Need VPN From Iran]", IsCustomTextAvailable = false});
        Data.Add(new ComboModel {Name = "Makhlas", IsCustomTextAvailable = false});
        Data.Add(new ComboModel {Name = "ReLink", IsCustomTextAvailable = false});
        Data.Add(new ComboModel {Name = "Shrturi", IsCustomTextAvailable = false});
        Data.Add(new ComboModel {Name = "Shrtco", IsCustomTextAvailable = false});

        LongUrlChangedCommand = new DelegateCommand<TextChangedEventArgs>(UrlChanged);
        ShortenCommand = new DelegateCommand<string>(Shorten);
        OpenFileCommand = new DelegateCommand(OpenFile);
        HelpCommand = new DelegateCommand(Help);
        StartCommand = new DelegateCommand<IList>(Start);
    }

    #region Properties

    private ObservableCollection<ComboModel> _data;

    public ObservableCollection<ComboModel> Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

    private bool _isButtonEnable;

    public bool IsButtonEnable
    {
        get => _isButtonEnable;
        set => SetProperty(ref _isButtonEnable, value);
    }

    private string _selectedCustomText;

    public string SelectedCustomText
    {
        get => _selectedCustomText;
        set => SetProperty(ref _selectedCustomText, value);
    }

    private int _selectedItemIndex;

    public int SelectedItemIndex
    {
        get => _selectedItemIndex;
        set
        {
            if (value == _selectedItemIndex)
            {
                return;
            }

            SetProperty(ref _selectedItemIndex, value);
            GlobalData.Config.SelectedIndex = value;
            GlobalData.Save();
        }
    }

    private string _shortedUrl;

    public string ShortedUrl
    {
        get => _shortedUrl;
        set => SetProperty(ref _shortedUrl, value);
    }

    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private ObservableCollection<ShorterListModel> _shorterList;

    public ObservableCollection<ShorterListModel> ShorterList
    {
        get => _shorterList;
        set => SetProperty(ref _shorterList, value);
    }

    #endregion


    #region Commands

    public DelegateCommand<TextChangedEventArgs> LongUrlChangedCommand { get; }
    public DelegateCommand<string> ShortenCommand { get; }
    public DelegateCommand OpenFileCommand { get; }
    public DelegateCommand HelpCommand { get; set; }
    public DelegateCommand<IList> StartCommand { get; }

    #endregion

    #region Methods

    private void UrlChanged(TextChangedEventArgs e)
    {
        if (e.OriginalSource is not HandyControl.Controls.TextBox item)
        {
            return;
        }

        if (string.IsNullOrEmpty(item.Text))
        {
            IsButtonEnable = false;
        }

        var result = Uri.TryCreate(item.Text, UriKind.Absolute, out var uriResult)
                     && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        IsButtonEnable = result;
    }

    private async void Shorten(string longUrl)
    {
        IsButtonEnable = false;
        ShortedUrl = SelectedItemIndex switch
        {
            0 => await OpizoShorten(longUrl),
            1 => await BitlyShorten(longUrl),
            2 => await PlinkShorten(longUrl),
            3 => await CuttlyShorten(longUrl),
            4 => await RebrandlyShorten(longUrl),
            5 => await TinyUrlShorten(longUrl),
            6 => await MakhlasShorten(longUrl),
            7 => await RelLinkShorten(longUrl),
            8 => await ShrturiShorten(longUrl),
            9 => await ShrtcoShorten(longUrl),
            _ => ShortedUrl
        };

        IsButtonEnable = true;

        if (!ShortedUrl.Contains("error"))
        {
            Clipboard.SetText(ShortedUrl);
        }
    }

    private async void Start(IList shorterSelectedList)
    {
        try
        {
            IsBusy = true;
            foreach (ShorterListModel shorterListModel in shorterSelectedList)
            {
                var longLink = shorterListModel.Link;

                switch (SelectedItemIndex)
                {
                    case 0:
                        ShorterList.Add(new ShorterListModel {ShortLink = await OpizoShorten(longLink)});
                        break;

                    case 1:
                        ShorterList.Add(new ShorterListModel {ShortLink = await BitlyShorten(longLink)});
                        break;

                    case 2:
                        ShorterList.Add(new ShorterListModel {ShortLink = await PlinkShorten(longLink)});
                        break;
                    case 3:
                        ShorterList.Add(new ShorterListModel {ShortLink = await CuttlyShorten(longLink)});
                        break;
                    case 4:
                        ShorterList.Add(new ShorterListModel {ShortLink = await RebrandlyShorten(longLink)});
                        break;
                    case 5:
                        ShorterList.Add(new ShorterListModel {ShortLink = await TinyUrlShorten(longLink)});
                        break;
                    case 6:
                        ShorterList.Add(new ShorterListModel {ShortLink = await MakhlasShorten(longLink)});
                        break;
                    case 7:
                        ShorterList.Add(new ShorterListModel {ShortLink = await RelLinkShorten(longLink)});
                        break;
                    case 8:
                        ShorterList.Add(new ShorterListModel {ShortLink = await ShrturiShorten(longLink)});
                        break;
                    case 9:
                        ShorterList.Add(new ShorterListModel {ShortLink = await ShrtcoShorten(longLink)});
                        break;
                }
            }

            IsBusy = false;
            var oDialog = new SaveFileDialog
            {
                Title = "Save Text File",
                Filter = "TXT files|*.txt"
            };

            if (oDialog.ShowDialog() == true)
            {
                await File.WriteAllLinesAsync(oDialog.FileName,
                    ShorterList.Where(x => !string.IsNullOrEmpty(x.ShortLink)).Select(x => x.ShortLink));
            }

            ShorterList = new ObservableCollection<ShorterListModel>();
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static void Help()
    {
        Growl.Info(new GrowlInfo
        {
            Message =
                "Step 1: Create Txt File like this(each line one Link): https://Test.com   https://Test.com   Step 2: Load txt File   Step 3: Choose Url Shorten Service    Step 4: Select urls   Step 5: Click Start",
            ShowDateTime = false
        });
    }

    private void OpenFile()
    {
        ShorterList.Clear();
        OpenFileDialog theDialog = new()
        {
            Title = "Open Text File",
            Filter = "TXT files|*.txt"
        };

        if (theDialog.ShowDialog() != true)
        {
            return;
        }

        var filename = theDialog.FileName;
        var lines = File.ReadAllLines(filename);

        foreach (var item in lines)
        {
            ShorterList.Add(new ShorterListModel {Link = item});
        }
    }

    private async Task<string> OpizoShorten(string longUrl)
    {
        try
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("url", longUrl)
            });

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-API-KEY", _opizoApiKey);
            var response = await client.PostAsync("https://opizo.com/api/v1/shrink/", content);
            var root = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result)!;

            string status = root.status;

            if (status.Equals("success"))
            {
                string link = root.data.url;
                return link;
            }

            string error = root.data;
            Growl.Error(error);
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }


        return "error";
    }

    private async Task<string> PlinkShorten(string longUrl)
    {
        try
        {
            var utf8 = new UTF8Encoding();
            var utf8Encoded = HttpUtility.UrlEncode(longUrl, utf8);
            using var client = new HttpClient();
            using var response = await client.GetAsync(
                $"https://plink.ir/api?api={_plinkApiKey}& url={utf8Encoded}&custom={SelectedCustomText}");
            using var content = response.Content;
            var root = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result)!;
            string error = root.error;

            if (error.Contains('0'))
            {
                string link = root.@short;
                return link;
            }

            string msg = root.msg;
            Growl.Error(msg);
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }

        return "error";
    }

    private async Task<string> BitlyShorten(string longUrl)
    {
        try
        {
            var data = new
            {
                group_guid = "Ba1bc23dE4F",
                domain = "bit.ly",
                long_url = longUrl
            };

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("authorization", $"Bearer {_bitlyApiKey}");

            using var response = await client.PostAsync("https://api-ssl.bitly.com/v4/shorten", data.AsJson());
            using var content = response.Content;
            var root = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result)!;

            string statusCode = root.statusCode;

            if (statusCode.Equals("OK"))
            {
                string link = root.results[longUrl].shortUrl;

                return link;
            }

            string error = root.errorMessage;
            Growl.Error(error);
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }


        return "error";
    }

    private async Task<string> CuttlyShorten(string longUrl)
    {
        try
        {
            var url =
                $"https://cutt.ly/api/api.php?key={_cuttlyApiKey}&short={HttpUtility.UrlEncode(longUrl)}&name={SelectedCustomText}";

            using var client = new HttpClient();
            using var response = await client.GetAsync(url);
            using var content = response.Content;
            var root = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result)!;

            string statusCode = root.url.status;
            if (statusCode.Equals("7"))
            {
                string link = root.url.shortLink;

                return link;
            }

            string error = root.status;
            Growl.Error(error);
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }


        return "error";
    }

    private async Task<string> RebrandlyShorten(string longUrl)
    {
        try
        {
            var data = new {destination = longUrl, slashtag = SelectedCustomText};

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", _rebrandlyApiKey);
            var response = await client.PostAsync("https://api.rebrandly.com/v1/links", data.AsJson());
            var root = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result)!;

            string link = root.shortUrl;

            if (link != null)
            {
                return link;
            }

            string error = root.message;
            Growl.Error(error);
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }

        return "error";
    }

    private static async Task<string> TinyUrlShorten(string longUrl)
    {
        try
        {
            var url = $"https://tinyurl.com/api-create.php?url={HttpUtility.UrlEncode(longUrl)}";

            using var client = new HttpClient();
            using var response = await client.GetAsync(url);
            using var content = response.Content;
            var root = response.Content.ReadAsStringAsync().Result;

            if (!root.Contains("Error"))
            {
                return root;
            }

            Growl.Error(root);
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }


        return "error";
    }

    private async Task<string> MakhlasShorten(string longUrl)
    {
        try
        {
            var data = new {long_url = longUrl};

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("authorization", $"Bearer {_makhlasApiKey}");

            var response = await client.PostAsync(
                "http://api.makhlas.com/v1/url", data.AsJson());
            var root = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result)!;

            string status = root.success;

            if (status.ToLower().Contains("true"))
            {
                string link = root.data[0].short_url;
                return link;
            }

            string error = root.detail;
            Growl.Error(error);
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }

        return "error";
    }

    private static async Task<string> RelLinkShorten(string longUrl)
    {
        try
        {
            var data = new {url = longUrl};

            using var client = new HttpClient();
            var response = await client.PostAsync("https://rel.ink/api/links/", data.AsJson());
            var root = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result)!;

            string link = root.hashid;
            if (link != null)
            {
                return $"https://rel.ink/{link}";
            }
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }

        return "error";
    }

    private static async Task<string> ShrturiShorten(string longUrl)
    {
        try
        {
            var data = new {url = longUrl};

            using var client = new HttpClient();
            var response = await client.PostAsync("https://shrturi.com/api/v1/shorten", data.AsJson());
            var result = response.Content.ReadAsStringAsync().Result;
            var root = JsonConvert.DeserializeObject<dynamic>(result)!;

            string link = root.result_url;
            if (link != null)
            {
                return link;
            }
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }

        return "error";
    }

    private static async Task<string> ShrtcoShorten(string longUrl)
    {
        try
        {
            var url = $"https://api.shrtco.de/v2/shorten?url={HttpUtility.UrlEncode(longUrl)}";

            using var client = new HttpClient();
            using var response = await client.GetAsync(url);
            using var content = response.Content;
            var root = response.Content.ReadAsStringAsync().Result;
            var parse = JsonConvert.DeserializeObject<dynamic>(root)!;
            string status = parse.ok;
            if (!status.Contains("true"))
            {
                string link = parse.result.full_short_link;
                return link;
            }
        }
        catch (Exception ex)
        {
            Growl.Error(ex.Message);
        }

        return "error";
    }

    #endregion
}