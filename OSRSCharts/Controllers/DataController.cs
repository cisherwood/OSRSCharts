using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace OSRSCharts.Controllers
{
    public class DataController : Controller
    {

        public IActionResult UpdateItem()
        {


            string json = System.IO.File.ReadAllText(@"wwwroot\data\items-complete.json");


            Items items = JsonConvert.DeserializeObject<Items>(json);

            return View();
        }
    }




    public partial class Items
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("incomplete")]
        public bool Incomplete { get; set; }

        [JsonProperty("members")]
        public bool Members { get; set; }

        [JsonProperty("tradeable")]
        public bool Tradeable { get; set; }

        [JsonProperty("tradeable_on_ge")]
        public bool TradeableOnGe { get; set; }

        [JsonProperty("stackable")]
        public bool Stackable { get; set; }

        [JsonProperty("noted")]
        public bool Noted { get; set; }

        [JsonProperty("noteable")]
        public bool Noteable { get; set; }

        [JsonProperty("linked_id_item")]
        public long? LinkedIdItem { get; set; }

        [JsonProperty("linked_id_noted")]
        public long? LinkedIdNoted { get; set; }

        [JsonProperty("linked_id_placeholder")]
        public long? LinkedIdPlaceholder { get; set; }

        [JsonProperty("placeholder")]
        public bool Placeholder { get; set; }

        [JsonProperty("equipable")]
        public bool Equipable { get; set; }

        [JsonProperty("equipable_by_player")]
        public bool EquipableByPlayer { get; set; }

        [JsonProperty("equipable_weapon")]
        public bool EquipableWeapon { get; set; }

        [JsonProperty("cost")]
        public long Cost { get; set; }

        [JsonProperty("lowalch")]
        public long Lowalch { get; set; }

        [JsonProperty("highalch")]
        public long Highalch { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }

        [JsonProperty("buy_limit")]
        public long? BuyLimit { get; set; }

        [JsonProperty("quest_item")]
        public bool QuestItem { get; set; }

        [JsonProperty("release_date")]
        public DateTimeOffset? ReleaseDate { get; set; }

        [JsonProperty("duplicate")]
        public bool Duplicate { get; set; }

        [JsonProperty("examine")]
        public string Examine { get; set; }

        [JsonProperty("wiki_name")]
        public string WikiName { get; set; }

        [JsonProperty("wiki_url")]
        public Uri WikiUrl { get; set; }

        [JsonProperty("equipment")]
        public Equipment Equipment { get; set; }

        [JsonProperty("weapon")]
        public Weapon Weapon { get; set; }
    }

    public partial class Equipment
    {
        [JsonProperty("attack_stab")]
        public long AttackStab { get; set; }

        [JsonProperty("attack_slash")]
        public long AttackSlash { get; set; }

        [JsonProperty("attack_crush")]
        public long AttackCrush { get; set; }

        [JsonProperty("attack_magic")]
        public long AttackMagic { get; set; }

        [JsonProperty("attack_ranged")]
        public long AttackRanged { get; set; }

        [JsonProperty("defence_stab")]
        public long DefenceStab { get; set; }

        [JsonProperty("defence_slash")]
        public long DefenceSlash { get; set; }

        [JsonProperty("defence_crush")]
        public long DefenceCrush { get; set; }

        [JsonProperty("defence_magic")]
        public long DefenceMagic { get; set; }

        [JsonProperty("defence_ranged")]
        public long DefenceRanged { get; set; }

        [JsonProperty("melee_strength")]
        public long MeleeStrength { get; set; }

        [JsonProperty("ranged_strength")]
        public long RangedStrength { get; set; }

        [JsonProperty("magic_damage")]
        public long MagicDamage { get; set; }

        [JsonProperty("prayer")]
        public long Prayer { get; set; }

        [JsonProperty("slot")]
        public Slot Slot { get; set; }

        [JsonProperty("requirements")]
        public Dictionary<string, long> Requirements { get; set; }
    }

    public partial class Weapon
    {
        [JsonProperty("attack_speed")]
        public long AttackSpeed { get; set; }

        [JsonProperty("weapon_type")]
        public WeaponType WeaponType { get; set; }

        [JsonProperty("stances")]
        public Stance[] Stances { get; set; }
    }

    public partial class Stance
    {
        [JsonProperty("combat_style")]
        public CombatStyle CombatStyle { get; set; }

        [JsonProperty("attack_type")]
        public AttackType? AttackType { get; set; }

        [JsonProperty("attack_style")]
        public AttackStyle? AttackStyle { get; set; }

        [JsonProperty("experience")]
        public Experience Experience { get; set; }

        [JsonProperty("boosts")]
        public Boosts? Boosts { get; set; }
    }

    public enum Slot { Ammo, Body, Cape, Feet, Hands, Head, Legs, Neck, Ring, Shield, The2H, Weapon };

    public enum AttackStyle { Accurate, Aggressive, Controlled, Defensive, Magic, None };

    public enum AttackType { Crush, DefensiveCasting, Magic, None, Ranged, Slash, Spellcasting, Stab };

    public enum Boosts { AccuracyAndDamage, AttackRangeBy2Squares, AttackSpeedBy1Tick };

    public enum CombatStyle { Accurate, AimAndFire, Bash, Blaze, Block, Chop, Deflect, Fend, Flare, Flick, Focus, Hack, Impale, Jab, Kick, Lash, LongFuse, Longrange, Lunge, MediumFuse, Pound, Pummel, Punch, Rapid, Reap, Scorch, ShortFuse, Slash, Smash, Spell, SpellDefensive, Spike, Stab, Swipe };

    public enum Experience { Attack, Defence, Magic, MagicAndDefence, None, Ranged, RangedAndDefence, Shared, Strength };

    public enum WeaponType { Axes, Banners, BladedStaves, BluntWeapons, Bows, Bulwarks, Chinchompas, Claws, Crossbows, Guns, Halberds, Pickaxes, Polestaves, Salamanders, Scythes, SlashingSwords, Spears, SpikedWeapons, StabbingSwords, Staves, ThrownWeapons, TridentClassWeapons, TwoHandedSwords, Unarmed, Whips };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                SlotConverter.Singleton,
                AttackStyleConverter.Singleton,
                AttackTypeConverter.Singleton,
                BoostsConverter.Singleton,
                CombatStyleConverter.Singleton,
                ExperienceConverter.Singleton,
                WeaponTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class SlotConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Slot) || t == typeof(Slot?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "2h":
                    return Slot.The2H;
                case "ammo":
                    return Slot.Ammo;
                case "body":
                    return Slot.Body;
                case "cape":
                    return Slot.Cape;
                case "feet":
                    return Slot.Feet;
                case "hands":
                    return Slot.Hands;
                case "head":
                    return Slot.Head;
                case "legs":
                    return Slot.Legs;
                case "neck":
                    return Slot.Neck;
                case "ring":
                    return Slot.Ring;
                case "shield":
                    return Slot.Shield;
                case "weapon":
                    return Slot.Weapon;
            }
            throw new Exception("Cannot unmarshal type Slot");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Slot)untypedValue;
            switch (value)
            {
                case Slot.The2H:
                    serializer.Serialize(writer, "2h");
                    return;
                case Slot.Ammo:
                    serializer.Serialize(writer, "ammo");
                    return;
                case Slot.Body:
                    serializer.Serialize(writer, "body");
                    return;
                case Slot.Cape:
                    serializer.Serialize(writer, "cape");
                    return;
                case Slot.Feet:
                    serializer.Serialize(writer, "feet");
                    return;
                case Slot.Hands:
                    serializer.Serialize(writer, "hands");
                    return;
                case Slot.Head:
                    serializer.Serialize(writer, "head");
                    return;
                case Slot.Legs:
                    serializer.Serialize(writer, "legs");
                    return;
                case Slot.Neck:
                    serializer.Serialize(writer, "neck");
                    return;
                case Slot.Ring:
                    serializer.Serialize(writer, "ring");
                    return;
                case Slot.Shield:
                    serializer.Serialize(writer, "shield");
                    return;
                case Slot.Weapon:
                    serializer.Serialize(writer, "weapon");
                    return;
            }
            throw new Exception("Cannot marshal type Slot");
        }

        public static readonly SlotConverter Singleton = new SlotConverter();
    }

    internal class AttackStyleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AttackStyle) || t == typeof(AttackStyle?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "accurate":
                    return AttackStyle.Accurate;
                case "aggressive":
                    return AttackStyle.Aggressive;
                case "controlled":
                    return AttackStyle.Controlled;
                case "defensive":
                    return AttackStyle.Defensive;
                case "magic":
                    return AttackStyle.Magic;
                case "none":
                    return AttackStyle.None;
            }
            throw new Exception("Cannot unmarshal type AttackStyle");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AttackStyle)untypedValue;
            switch (value)
            {
                case AttackStyle.Accurate:
                    serializer.Serialize(writer, "accurate");
                    return;
                case AttackStyle.Aggressive:
                    serializer.Serialize(writer, "aggressive");
                    return;
                case AttackStyle.Controlled:
                    serializer.Serialize(writer, "controlled");
                    return;
                case AttackStyle.Defensive:
                    serializer.Serialize(writer, "defensive");
                    return;
                case AttackStyle.Magic:
                    serializer.Serialize(writer, "magic");
                    return;
                case AttackStyle.None:
                    serializer.Serialize(writer, "none");
                    return;
            }
            throw new Exception("Cannot marshal type AttackStyle");
        }

        public static readonly AttackStyleConverter Singleton = new AttackStyleConverter();
    }

    internal class AttackTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AttackType) || t == typeof(AttackType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "crush":
                    return AttackType.Crush;
                case "defensive casting":
                    return AttackType.DefensiveCasting;
                case "magic":
                    return AttackType.Magic;
                case "none":
                    return AttackType.None;
                case "ranged":
                    return AttackType.Ranged;
                case "slash":
                    return AttackType.Slash;
                case "spellcasting":
                    return AttackType.Spellcasting;
                case "stab":
                    return AttackType.Stab;
            }
            throw new Exception("Cannot unmarshal type AttackType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AttackType)untypedValue;
            switch (value)
            {
                case AttackType.Crush:
                    serializer.Serialize(writer, "crush");
                    return;
                case AttackType.DefensiveCasting:
                    serializer.Serialize(writer, "defensive casting");
                    return;
                case AttackType.Magic:
                    serializer.Serialize(writer, "magic");
                    return;
                case AttackType.None:
                    serializer.Serialize(writer, "none");
                    return;
                case AttackType.Ranged:
                    serializer.Serialize(writer, "ranged");
                    return;
                case AttackType.Slash:
                    serializer.Serialize(writer, "slash");
                    return;
                case AttackType.Spellcasting:
                    serializer.Serialize(writer, "spellcasting");
                    return;
                case AttackType.Stab:
                    serializer.Serialize(writer, "stab");
                    return;
            }
            throw new Exception("Cannot marshal type AttackType");
        }

        public static readonly AttackTypeConverter Singleton = new AttackTypeConverter();
    }

    internal class BoostsConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Boosts) || t == typeof(Boosts?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "accuracy and damage":
                    return Boosts.AccuracyAndDamage;
                case "attack range by 2 squares":
                    return Boosts.AttackRangeBy2Squares;
                case "attack speed by 1 tick":
                    return Boosts.AttackSpeedBy1Tick;
            }
            throw new Exception("Cannot unmarshal type Boosts");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Boosts)untypedValue;
            switch (value)
            {
                case Boosts.AccuracyAndDamage:
                    serializer.Serialize(writer, "accuracy and damage");
                    return;
                case Boosts.AttackRangeBy2Squares:
                    serializer.Serialize(writer, "attack range by 2 squares");
                    return;
                case Boosts.AttackSpeedBy1Tick:
                    serializer.Serialize(writer, "attack speed by 1 tick");
                    return;
            }
            throw new Exception("Cannot marshal type Boosts");
        }

        public static readonly BoostsConverter Singleton = new BoostsConverter();
    }

    internal class CombatStyleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(CombatStyle) || t == typeof(CombatStyle?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "accurate":
                    return CombatStyle.Accurate;
                case "aim and fire":
                    return CombatStyle.AimAndFire;
                case "bash":
                    return CombatStyle.Bash;
                case "blaze":
                    return CombatStyle.Blaze;
                case "block":
                    return CombatStyle.Block;
                case "chop":
                    return CombatStyle.Chop;
                case "deflect":
                    return CombatStyle.Deflect;
                case "fend":
                    return CombatStyle.Fend;
                case "flare":
                    return CombatStyle.Flare;
                case "flick":
                    return CombatStyle.Flick;
                case "focus":
                    return CombatStyle.Focus;
                case "hack":
                    return CombatStyle.Hack;
                case "impale":
                    return CombatStyle.Impale;
                case "jab":
                    return CombatStyle.Jab;
                case "kick":
                    return CombatStyle.Kick;
                case "lash":
                    return CombatStyle.Lash;
                case "long fuse":
                    return CombatStyle.LongFuse;
                case "longrange":
                    return CombatStyle.Longrange;
                case "lunge":
                    return CombatStyle.Lunge;
                case "medium fuse":
                    return CombatStyle.MediumFuse;
                case "pound":
                    return CombatStyle.Pound;
                case "pummel":
                    return CombatStyle.Pummel;
                case "punch":
                    return CombatStyle.Punch;
                case "rapid":
                    return CombatStyle.Rapid;
                case "reap":
                    return CombatStyle.Reap;
                case "scorch":
                    return CombatStyle.Scorch;
                case "short fuse":
                    return CombatStyle.ShortFuse;
                case "slash":
                    return CombatStyle.Slash;
                case "smash":
                    return CombatStyle.Smash;
                case "spell":
                    return CombatStyle.Spell;
                case "spell (defensive)":
                    return CombatStyle.SpellDefensive;
                case "spike":
                    return CombatStyle.Spike;
                case "stab":
                    return CombatStyle.Stab;
                case "swipe":
                    return CombatStyle.Swipe;
            }
            throw new Exception("Cannot unmarshal type CombatStyle");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (CombatStyle)untypedValue;
            switch (value)
            {
                case CombatStyle.Accurate:
                    serializer.Serialize(writer, "accurate");
                    return;
                case CombatStyle.AimAndFire:
                    serializer.Serialize(writer, "aim and fire");
                    return;
                case CombatStyle.Bash:
                    serializer.Serialize(writer, "bash");
                    return;
                case CombatStyle.Blaze:
                    serializer.Serialize(writer, "blaze");
                    return;
                case CombatStyle.Block:
                    serializer.Serialize(writer, "block");
                    return;
                case CombatStyle.Chop:
                    serializer.Serialize(writer, "chop");
                    return;
                case CombatStyle.Deflect:
                    serializer.Serialize(writer, "deflect");
                    return;
                case CombatStyle.Fend:
                    serializer.Serialize(writer, "fend");
                    return;
                case CombatStyle.Flare:
                    serializer.Serialize(writer, "flare");
                    return;
                case CombatStyle.Flick:
                    serializer.Serialize(writer, "flick");
                    return;
                case CombatStyle.Focus:
                    serializer.Serialize(writer, "focus");
                    return;
                case CombatStyle.Hack:
                    serializer.Serialize(writer, "hack");
                    return;
                case CombatStyle.Impale:
                    serializer.Serialize(writer, "impale");
                    return;
                case CombatStyle.Jab:
                    serializer.Serialize(writer, "jab");
                    return;
                case CombatStyle.Kick:
                    serializer.Serialize(writer, "kick");
                    return;
                case CombatStyle.Lash:
                    serializer.Serialize(writer, "lash");
                    return;
                case CombatStyle.LongFuse:
                    serializer.Serialize(writer, "long fuse");
                    return;
                case CombatStyle.Longrange:
                    serializer.Serialize(writer, "longrange");
                    return;
                case CombatStyle.Lunge:
                    serializer.Serialize(writer, "lunge");
                    return;
                case CombatStyle.MediumFuse:
                    serializer.Serialize(writer, "medium fuse");
                    return;
                case CombatStyle.Pound:
                    serializer.Serialize(writer, "pound");
                    return;
                case CombatStyle.Pummel:
                    serializer.Serialize(writer, "pummel");
                    return;
                case CombatStyle.Punch:
                    serializer.Serialize(writer, "punch");
                    return;
                case CombatStyle.Rapid:
                    serializer.Serialize(writer, "rapid");
                    return;
                case CombatStyle.Reap:
                    serializer.Serialize(writer, "reap");
                    return;
                case CombatStyle.Scorch:
                    serializer.Serialize(writer, "scorch");
                    return;
                case CombatStyle.ShortFuse:
                    serializer.Serialize(writer, "short fuse");
                    return;
                case CombatStyle.Slash:
                    serializer.Serialize(writer, "slash");
                    return;
                case CombatStyle.Smash:
                    serializer.Serialize(writer, "smash");
                    return;
                case CombatStyle.Spell:
                    serializer.Serialize(writer, "spell");
                    return;
                case CombatStyle.SpellDefensive:
                    serializer.Serialize(writer, "spell (defensive)");
                    return;
                case CombatStyle.Spike:
                    serializer.Serialize(writer, "spike");
                    return;
                case CombatStyle.Stab:
                    serializer.Serialize(writer, "stab");
                    return;
                case CombatStyle.Swipe:
                    serializer.Serialize(writer, "swipe");
                    return;
            }
            throw new Exception("Cannot marshal type CombatStyle");
        }

        public static readonly CombatStyleConverter Singleton = new CombatStyleConverter();
    }

    internal class ExperienceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Experience) || t == typeof(Experience?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "attack":
                    return Experience.Attack;
                case "defence":
                    return Experience.Defence;
                case "magic":
                    return Experience.Magic;
                case "magic and defence":
                    return Experience.MagicAndDefence;
                case "none":
                    return Experience.None;
                case "ranged":
                    return Experience.Ranged;
                case "ranged and defence":
                    return Experience.RangedAndDefence;
                case "shared":
                    return Experience.Shared;
                case "strength":
                    return Experience.Strength;
            }
            throw new Exception("Cannot unmarshal type Experience");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Experience)untypedValue;
            switch (value)
            {
                case Experience.Attack:
                    serializer.Serialize(writer, "attack");
                    return;
                case Experience.Defence:
                    serializer.Serialize(writer, "defence");
                    return;
                case Experience.Magic:
                    serializer.Serialize(writer, "magic");
                    return;
                case Experience.MagicAndDefence:
                    serializer.Serialize(writer, "magic and defence");
                    return;
                case Experience.None:
                    serializer.Serialize(writer, "none");
                    return;
                case Experience.Ranged:
                    serializer.Serialize(writer, "ranged");
                    return;
                case Experience.RangedAndDefence:
                    serializer.Serialize(writer, "ranged and defence");
                    return;
                case Experience.Shared:
                    serializer.Serialize(writer, "shared");
                    return;
                case Experience.Strength:
                    serializer.Serialize(writer, "strength");
                    return;
            }
            throw new Exception("Cannot marshal type Experience");
        }

        public static readonly ExperienceConverter Singleton = new ExperienceConverter();
    }

    internal class WeaponTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(WeaponType) || t == typeof(WeaponType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "axes":
                    return WeaponType.Axes;
                case "banners":
                    return WeaponType.Banners;
                case "bladed_staves":
                    return WeaponType.BladedStaves;
                case "blunt_weapons":
                    return WeaponType.BluntWeapons;
                case "bows":
                    return WeaponType.Bows;
                case "bulwarks":
                    return WeaponType.Bulwarks;
                case "chinchompas":
                    return WeaponType.Chinchompas;
                case "claws":
                    return WeaponType.Claws;
                case "crossbows":
                    return WeaponType.Crossbows;
                case "guns":
                    return WeaponType.Guns;
                case "halberds":
                    return WeaponType.Halberds;
                case "pickaxes":
                    return WeaponType.Pickaxes;
                case "polestaves":
                    return WeaponType.Polestaves;
                case "salamanders":
                    return WeaponType.Salamanders;
                case "scythes":
                    return WeaponType.Scythes;
                case "slashing_swords":
                    return WeaponType.SlashingSwords;
                case "spears":
                    return WeaponType.Spears;
                case "spiked_weapons":
                    return WeaponType.SpikedWeapons;
                case "stabbing_swords":
                    return WeaponType.StabbingSwords;
                case "staves":
                    return WeaponType.Staves;
                case "thrown_weapons":
                    return WeaponType.ThrownWeapons;
                case "trident-class_weapons":
                    return WeaponType.TridentClassWeapons;
                case "two-handed_swords":
                    return WeaponType.TwoHandedSwords;
                case "unarmed":
                    return WeaponType.Unarmed;
                case "whips":
                    return WeaponType.Whips;
            }
            throw new Exception("Cannot unmarshal type WeaponType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (WeaponType)untypedValue;
            switch (value)
            {
                case WeaponType.Axes:
                    serializer.Serialize(writer, "axes");
                    return;
                case WeaponType.Banners:
                    serializer.Serialize(writer, "banners");
                    return;
                case WeaponType.BladedStaves:
                    serializer.Serialize(writer, "bladed_staves");
                    return;
                case WeaponType.BluntWeapons:
                    serializer.Serialize(writer, "blunt_weapons");
                    return;
                case WeaponType.Bows:
                    serializer.Serialize(writer, "bows");
                    return;
                case WeaponType.Bulwarks:
                    serializer.Serialize(writer, "bulwarks");
                    return;
                case WeaponType.Chinchompas:
                    serializer.Serialize(writer, "chinchompas");
                    return;
                case WeaponType.Claws:
                    serializer.Serialize(writer, "claws");
                    return;
                case WeaponType.Crossbows:
                    serializer.Serialize(writer, "crossbows");
                    return;
                case WeaponType.Guns:
                    serializer.Serialize(writer, "guns");
                    return;
                case WeaponType.Halberds:
                    serializer.Serialize(writer, "halberds");
                    return;
                case WeaponType.Pickaxes:
                    serializer.Serialize(writer, "pickaxes");
                    return;
                case WeaponType.Polestaves:
                    serializer.Serialize(writer, "polestaves");
                    return;
                case WeaponType.Salamanders:
                    serializer.Serialize(writer, "salamanders");
                    return;
                case WeaponType.Scythes:
                    serializer.Serialize(writer, "scythes");
                    return;
                case WeaponType.SlashingSwords:
                    serializer.Serialize(writer, "slashing_swords");
                    return;
                case WeaponType.Spears:
                    serializer.Serialize(writer, "spears");
                    return;
                case WeaponType.SpikedWeapons:
                    serializer.Serialize(writer, "spiked_weapons");
                    return;
                case WeaponType.StabbingSwords:
                    serializer.Serialize(writer, "stabbing_swords");
                    return;
                case WeaponType.Staves:
                    serializer.Serialize(writer, "staves");
                    return;
                case WeaponType.ThrownWeapons:
                    serializer.Serialize(writer, "thrown_weapons");
                    return;
                case WeaponType.TridentClassWeapons:
                    serializer.Serialize(writer, "trident-class_weapons");
                    return;
                case WeaponType.TwoHandedSwords:
                    serializer.Serialize(writer, "two-handed_swords");
                    return;
                case WeaponType.Unarmed:
                    serializer.Serialize(writer, "unarmed");
                    return;
                case WeaponType.Whips:
                    serializer.Serialize(writer, "whips");
                    return;
            }
            throw new Exception("Cannot marshal type WeaponType");
        }

        public static readonly WeaponTypeConverter Singleton = new WeaponTypeConverter();
    }





}







