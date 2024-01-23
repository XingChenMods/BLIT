﻿using BLIT.scripts.Common;
using MessagePack;
using Serilog;
using System;
using System.Collections.Generic;

namespace BLIT.scripts.Models.BannerIcons;

[MessagePackObject]
public class BannerSettings {
    [Key(0)]
    public List<string> SpriteScanFolders { get; set; } = new();
    [Key(1)]
    public Banner.OutputResolution TextureOutputResolution { get; set; }

    private int _customGroupStartID = 7;
    [Key(2)]
    public int CustomGroupStartID {
        get => _customGroupStartID;
        set {
            if (_customGroupStartID == value) {
                return;
            }

            _customGroupStartID = value;
            Save();
        }
    }

    private int _customColorStartID = 194;
    [Key(3)]
    public int CustomColorStartID {
        get => _customColorStartID;
        set {
            if (_customColorStartID == value) {
                return;
            }

            _customColorStartID = value;
            Save();
        }
    }
    public void SaveSpriteScanFolders(IEnumerable<string> scanFolders) {
        SpriteScanFolders = new(scanFolders);
        Save();
    }

    public void Save() {
        var data = MessagePackSerializer.Serialize(this);
        Log.Debug("Saving banner settings: {Data}", MessagePackSerializer.ConvertToJson(data));
        var savedSettings = Convert.ToBase64String(data);
        AppConfig.Current.SetValue("BannerIcons", "BannerSettings", savedSettings);
        AppConfig.Save();
    }
    public static BannerSettings Load() {
        var savedSettings = AppConfig.Current.GetValue("BannerIcons", "BannerSettings", "").AsString();
        if (string.IsNullOrEmpty(savedSettings)) return new BannerSettings();
        var data = Convert.FromBase64String(savedSettings);
        Log.Debug("Loaded stored banner settings: {Data}", MessagePackSerializer.ConvertToJson(data));
        return MessagePackSerializer.Deserialize<BannerSettings>(data);
    }
}
