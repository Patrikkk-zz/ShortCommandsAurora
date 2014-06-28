using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace ShortCommandsV2
{
    [ApiVersion(1, 16)]
    public class SCommands : TerrariaPlugin
    {
        public override string Name { get { return "ShortCommands"; } }
        public override string Author { get { return "Zaicon"; } }
        public override string Description { get { return "A hardcoded version of ShortCommands, along with a few extra commands."; } }
        public override Version Version { get { return new Version("6.0"); } }
        public static Color acolor;
        public DateTime lastupdate = DateTime.Now;

        public SCPlayer[] Players { get; set; }
        
        public SCommands(Main game)
			: base(game)
		{
			base.Order = 1;
            Players = new SCPlayer[256];
		}

        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreet);
            ServerApi.Hooks.GameUpdate.Register(this, OnUpdate);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnGreet);
                ServerApi.Hooks.GameUpdate.Deregister(this, OnUpdate);
            }
            base.Dispose(Disposing);
        }

        public void OnInitialize(EventArgs args)
        {
            List<string> perms = new List<string> {"sc.check"};
            TShock.Groups.AddPermissions("griefwatch", perms);

            Commands.ChatCommands.Add(new Command(Permissions.manageregion, SCRegion1, "r1") { AllowServer = false, HelpText = "ShortCommand for /region set 1" });
            Commands.ChatCommands.Add(new Command(Permissions.manageregion, SCRegion2, "r2") { AllowServer = false, HelpText = "ShortCommand for /region set 2" });
            Commands.ChatCommands.Add(new Command(Permissions.manageregion, SCRegion3, "rd") { AllowServer = false, HelpText = "ShortCommand for /region define <region name>" });
            Commands.ChatCommands.Add(new Command(Permissions.manageregion, SCRegion4, "rn") { AllowServer = false, HelpText = "ShortCommand for /region name" });
            Commands.ChatCommands.Add(new Command(Permissions.manageregion, SCRegion5, "ri") { AllowServer = false, HelpText = "ShortCommand for /region info <region name>" });
            Commands.ChatCommands.Add(new Command(Permissions.godmode, SCGod, "gm") { HelpText = "ShortCommand for /godmode" });
            Commands.ChatCommands.Add(new Command("history.get", SCHist, "h") { AllowServer = false, HelpText = "ShortCommand for /history" });
            Commands.ChatCommands.Add(new Command("history.rollback", SCRoll, "rb") { AllowServer = false, HelpText = "ShortCommand for /rollback <name> <time> <radius>" });
            Commands.ChatCommands.Add(new Command("tshock.world.modify", SCRGrief, "rg") { AllowServer = false, HelpText = "ShortCommand for /reportgrief" });
            Commands.ChatCommands.Add(new Command("aio.checkgrief", SCCGrief, "cg") { AllowServer = false, HelpText = "ShortCommand for /checkgrief" });
            Commands.ChatCommands.Add(new Command("aio.checkbuilding", SCCBuilding, "cb") { AllowServer = false, HelpText = "ShortCommand for /checkbuilding" });
            Commands.ChatCommands.Add(new Command("tshockirc.manage", SCIRC, "irc") { HelpText = "ShortCommand for /ircrestart" });
            Commands.ChatCommands.Add(new Command("sendcolor", SCSColor, "sc") { HelpText = "ShortCommand for /sendcolor <color> <text>" });
            Commands.ChatCommands.Add(new Command("sc.clear", SCClear, "ci") { HelpText = "ShortCommand for /clear item 30000" });
            Commands.ChatCommands.Add(new Command("worldedit.selection.point", SCPoint1, "p1") { AllowServer = false, HelpText = "ShortCommand for //point 1" });
            Commands.ChatCommands.Add(new Command("worldedit.selection.point", SCPoint2, "p2") { AllowServer = false, HelpText = "ShortCommand for //point 2" });
            Commands.ChatCommands.Add(new Command(Permissions.ban, SCBan, "gban") { HelpText = "ShortCommand for /oban add <name> Griefing, appeal at aurora-terraria.org" });
            Commands.ChatCommands.Add(new Command(SCList, "shortcommands") { AllowServer = false, HelpText = "Lists all ShortCommands available to you." });
            Commands.ChatCommands.Add(new Command("sc.pets", SCBunny, "bunny") { AllowServer = false, HelpText = "Spawns a Bunny pet!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCPenguin, "peng", "penguin") { AllowServer = false, HelpText = "Spawns a Penguin pet!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCTruffle, "truffle") { AllowServer = false, HelpText = "Spawns a baby Truffle pet!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCWisp, "wisp") { AllowServer = false, HelpText = "Spawns a Wisp!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCRudolph, "rudolph") { AllowServer = false, HelpText = "Spawns a Rudolph mount!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCFairy, "fairy") { AllowServer = false, HelpText = "Spawns a fairy!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCBunnyMount, "bunnymount") { AllowServer = false, HelpText = "Spawns a Bunny mount!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCSlimeMount, "slimemount") { AllowServer = false, HelpText = "Spawns a Slime mount!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCBeeMount, "beemount") { AllowServer = false, HelpText = "Spawns a Hornet mount!" });
            Commands.ChatCommands.Add(new Command("sc.buffs", SCBuffs, "selfbuff") { AllowServer = false, HelpText = "Buffs the user with a defined buff." });
            Commands.ChatCommands.Add(new Command("sc.poke", SCPoke, "poke") { HelpText = "Pokes a player!" });
            Commands.ChatCommands.Add(new Command("sc.supoke", SCSuPoke, "supoke") { HelpText = "Super pokes a player." });
            Commands.ChatCommands.Add(new Command("sc.stab", SCStab, "stab") { HelpText = "Stabs a player!" });
            Commands.ChatCommands.Add(new Command("sc.hug", SCHug, "hug") { HelpText = "Hugs a player!" });
            Commands.ChatCommands.Add(new Command("sc.lick", SCLick, "lick") { HelpText = "Licks a player!" });
            Commands.ChatCommands.Add(new Command("sc.face", SCFPalm, "facepalm") { HelpText = "Performs a facepalm." });
            Commands.ChatCommands.Add(new Command("sc.face", SCFDesk, "facedesk") { HelpText = "Slams your face on a desk." });
            Commands.ChatCommands.Add(new Command("sc.face", SCFPlant, "faceplant") { HelpText = "Slams your face on a plant. Err... makes you faceplant." });
            Commands.ChatCommands.Add(new Command("sc.face", SCFWall, "facewall") { HelpText = "You run into a wall at high speeds!" });
            Commands.ChatCommands.Add(new Command("sc.face", SCFBook, "facebook") { HelpText = "Checks your friends' status messages." });
            Commands.ChatCommands.Add(new Command("sc.slapall", SCSlapAll, "slapall") { HelpText = "Slaps ALL the people!" });
            Commands.ChatCommands.Add(new Command("sc.rape", SCRape, "rape") { HelpText = "Rapes the given player." });
            Commands.ChatCommands.Add(new Command("sc.sb", SCSB, "boss") { HelpText = "Spawns bosses.", AllowServer = false });
            Commands.ChatCommands.Add(new Command("sc.sm", SCSM, "mob") { HelpText = "Spawns mobs.", AllowServer = false });
            Commands.ChatCommands.Add(new Command("sc.bn", SCBN, "bn") { HelpText = "Butchers NPCs in a nearby radius.", AllowServer = false });
            Commands.ChatCommands.Add(new Command("sc.ranks", SCUser, "upgrade") { HelpText = "Adds user to User+ group." });
            Commands.ChatCommands.Add(new Command("sc.ranks", SCBuilder1, "b1") { HelpText = "Adds user to Builder 1 group." });
            Commands.ChatCommands.Add(new Command("sc.ranks", SCBuilder1, "b2") { HelpText = "Adds user to Builder 2 group." });
            Commands.ChatCommands.Add(new Command(SCWebsite, "website") { HelpText = "The website for Aurora Terraria." });
            Commands.ChatCommands.Add(new Command("tshock.world.modify", SCInfo, "ranks") { HelpText = "Shows information on ranks."});
            Commands.ChatCommands.Add(new Command(SCCheck, "check") { HelpText = "Checks to see if the specified username is taken." });
        }

        public void OnUpdate(EventArgs args)
        {
            if ((DateTime.UtcNow - lastupdate).TotalSeconds >= 1)
            {
                lastupdate = DateTime.UtcNow;
                lock (Players)
                    foreach (SCPlayer player in Players)
                    {
                        if (player != null && player.TSPlayer != null)
                        {
                            if (player.needsUpdating || player.lastBuffUpdate.TimeOfDay.TotalMinutes < (DateTime.Now.TimeOfDay.TotalMinutes - 3))
                            {
                                if (player.hasShine)
                                    player.TSPlayer.SetBuff(11, 18000);
                                if (player.hasSkin)
                                    player.TSPlayer.SetBuff(1, 18000);
                                if (player.hasGills)
                                    player.TSPlayer.SetBuff(4, 18000);
                                if (player.hasInvis)
                                    player.TSPlayer.SetBuff(10, 18000);
                                if (player.hasWaterW)
                                    player.TSPlayer.SetBuff(15, 18000);
                                if (player.hasGrav)
                                    player.TSPlayer.SetBuff(18, 18000);
                                if (player.hasLife)
                                    player.TSPlayer.SetBuff(113, 18000);
                                if (player.hasFall)
                                    player.TSPlayer.SetBuff(8, 18000);
                                if (player.hasOrb)
                                    player.TSPlayer.SetBuff(19, 18000);
                                if (player.hasRedFairy)
                                    player.TSPlayer.SetBuff(101, 18000);
                                if (player.hasGreenFairy)
                                    player.TSPlayer.SetBuff(102, 18000);
                                if (player.hasBlueFairy)
                                    player.TSPlayer.SetBuff(27, 18000);

                                player.lastBuffUpdate = DateTime.Now;
                            }
                        }
                    }
            }
        }

        #region Check
        public void OnGreet(GreetPlayerEventArgs args)
        {
            Commands.HandleCommand(TShock.Players[args.Who], "/check start");

            Players[args.Who] = new SCPlayer(args.Who);
        }

        private void SCCheck(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid syntax: /check <player name>");
            }
            else if (args.Parameters[0] == "start" && !args.Player.IsLoggedIn)
            {
                if (TShock.Users.GetUserByName(args.Player.Name) == null)
                    args.Player.SendMessage("This character name is available! Please type /register <password> in order to claim this account as yours and give yourself the ability to build and use extra commands!", Color.LawnGreen);
                else
                    args.Player.SendMessage("This character name is already registered. If this is your account, please /login <password>. If you did not register this account, please make a new character with a new name and try again.", Color.LawnGreen);
            }
            else if (args.Parameters.Count != 0 && args.Player.Group.HasPermission("sc.check") && args.Parameters[0] != "start")
            {
                string plr = "";
                int count = 0;
                while (count < args.Parameters.Count)
                {
                    plr += args.Parameters[count];
                    count++;
                }
                if (TShock.Users.GetUserByName(plr) == null)
                    args.Player.SendMessage("\"" + plr + "\" is available.", Color.LawnGreen);
                else
                    args.Player.SendMessage("\"" + plr + "\" is not available.", Color.LawnGreen);
            }
        }
        #endregion

        #region Regions
        private void SCRegion1(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/region set 1");
        }
        private void SCRegion2(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/region set 2");
        }
        private void SCRegion3(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid Syntax: /rd <name>");
            }
            else if (args.Parameters.Count == 1)
                Commands.HandleCommand(args.Player, "/region define " + args.Parameters[0]);
            else
                args.Player.SendErrorMessage("Keep region names to a single word!");
        }
        private void SCRegion4(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/region name");
        }
        private void SCRegion5(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid Syntax: /ri <name>");
                return;
            }
            if (args.Parameters.Count > 1)
            {
                string cmd = "";
                if (!args.Parameters[0].StartsWith("\""))
                    cmd = "\"";
                for (int i = 0; i < args.Parameters.Count; i++)
                {
                    if (i != 0)
                        cmd += " ";
                    cmd += args.Parameters[i];
                }
                if (!args.Parameters[args.Parameters.Count - 1].EndsWith("\""))
                    cmd += "\"";
                Commands.HandleCommand(args.Player, "/region info " + cmd);
            }
            else
                Commands.HandleCommand(args.Player, "/region info " + args.Parameters[0]);
        }
        #endregion

        #region More Shortcommands
        private void SCGod(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
                Commands.HandleCommand(args.Player, "/godmode");
            else if (args.Parameters.Count == 1)
                Commands.HandleCommand(args.Player, "/godmode " + args.Parameters[0]);
            else if (args.Parameters.Count == 2)
                Commands.HandleCommand(args.Player, "/godmode " + args.Parameters[0] + " " + args.Parameters[1]);
            else
                args.Player.SendErrorMessage("Use /godmode for players with more than two words in his/her name.");
        }

        private void SCHist(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/history");
        }

        private void SCRoll(CommandArgs args)
        {
            if (args.Parameters.Count != 3)
            {
                args.Player.SendErrorMessage("Invalid Syntax: /rb <name> <time> <radius>");
                args.Player.SendErrorMessage("Use /rollback for players with more than one word in his/her name.");
            }
            else
                Commands.HandleCommand(args.Player, "/rollback " + args.Parameters[0] + " " + args.Parameters[1] + " " + args.Parameters[2]);
        }

        private void SCRGrief(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/reportgrief");
        }

        private void SCCGrief(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/checkgrief");
        }

        private void SCCBuilding(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/checkbuilding");
        }

        private void SCIRC(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/ircrestart");
        }

        private void SCSColor(CommandArgs args)
        {
            string cmd = "";
            for (int i = 0; i < args.Parameters.Count; i++)
            {
                if (i != 0)
                    cmd += " ";
                cmd += args.Parameters[i];
            }
            Commands.HandleCommand(args.Player, "/sendcolor " + cmd);
        }

        private void SCPoint1(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "//point 1");
        }

        private void SCPoint2(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "//point 2");
        }

        private void SCClear(CommandArgs args)
        {
            Commands.HandleCommand(args.Player, "/clear item 30000");
        }

        private void SCList(CommandArgs args)
        {
            args.Player.SendInfoMessage("List of available ShortCommands:");
            if (args.Player.Group.HasPermission("tshock.world.modify"))
                args.Player.SendInfoMessage("/rg: /reportgrief");
            if (args.Player.Group.HasPermission(Permissions.manageregion))
            {
                args.Player.SendInfoMessage("/r1: /region set 1");
                args.Player.SendInfoMessage("/r2: /region set 2");
                args.Player.SendInfoMessage("/rd: /region define <name>");
                args.Player.SendInfoMessage("/rn: /region name");
                args.Player.SendInfoMessage("/ri: /region info <name>");
            }
            if (args.Player.Group.HasPermission("history.get"))
            {
                args.Player.SendInfoMessage("/h: /history");
                args.Player.SendInfoMessage("/ci: /clear item 30000");
            }
            if (args.Player.Group.HasPermission("history.rollback"))
                args.Player.SendInfoMessage("/rb: /rollback <name> ");
            if (args.Player.Group.HasPermission("aio.checkgrief"))
                args.Player.SendInfoMessage("/cg: /checkgrief");
            if (args.Player.Group.HasPermission("aio.checkbuilding"))
                args.Player.SendInfoMessage("/cb: /checkbuilding");
            if (args.Player.Group.HasPermission(Permissions.godmode))
                args.Player.SendInfoMessage("/gm: /godmode [player]");
            if (args.Player.Group.HasPermission("tshockirc.manage"))
                args.Player.SendInfoMessage("/irc: /ircrestart");
            if (args.Player.Group.HasPermission("sendcolor"))
                args.Player.SendInfoMessage("/sc: /sendcolor <color> <message>");
            if (args.Player.Group.HasPermission("worldedit.selection.point"))
            {
                args.Player.SendInfoMessage("/p1: //point 1");
                args.Player.SendInfoMessage("/p2: //point 2");
            }
        }

        private void SCBan(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
                args.Player.SendErrorMessage("Invalid Syntax! /gban <griefer>");
            else if (args.Parameters.Count == 1)
            {
                Commands.HandleCommand(args.Player, "/oban add " + args.Parameters[0] + " Griefing, appeal at aurora-terraria.org/unban/");
            }
            else
                args.Player.SendErrorMessage("Invalid Syntax! /gban \"griefer name\"");
        }
        #endregion

        #region Pets
        private void SCBunny(CommandArgs args)
        {
            args.Player.SetBuff(40);
            args.Player.SendSuccessMessage("You spawned a pet Bunny! Right-click the bunny icon under your toolbar to deactivate.");
        }

        private void SCPenguin(CommandArgs args)
        {
            args.Player.SetBuff(41);
            args.Player.SendSuccessMessage("You spawned a pet Penguin! Right-click the penguin icon under your toolbar to deactivate.");
        }

        private void SCTruffle(CommandArgs args)
        {
            args.Player.SetBuff(55);
            args.Player.SendSuccessMessage("You spawned a pet Truffle! Right-click the truffle icon under your toolbar to deactivate.");
        }

        private void SCFairy(CommandArgs args)
        {
            if (args.Parameters.Count == 1)
            {
                if (args.Parameters[0].ToLower() == "blue")
                {
                    args.Player.SetBuff(27);
                    args.Player.SendSuccessMessage("You spawned a pet Fairy (blue)! Right-click the fairy icon under your toolbar to deactivate.");
                }
                else if (args.Parameters[0].ToLower() == "red")
                {
                    args.Player.SetBuff(101);
                    args.Player.SendSuccessMessage("You spawned a pet Fairy (red)! Right-click the fairy icon under your toolbar to deactivate.");
                }
                else if (args.Parameters[0].ToLower() == "green")
                {
                    args.Player.SetBuff(101);
                    args.Player.SendSuccessMessage("You spawned a pet Fairy (green)! Right-click the fairy icon under your toolbar to deactivate.");
                }
                else
                    args.Player.SendErrorMessage("Invalid fairy color! Use /fairy <blue/red/green>");
            }
            else
            {
                args.Player.SetBuff(27);
                args.Player.SendSuccessMessage("You spawned a pet Fairy (blue)! Right-click the fairy icon under your toolbar to deactivate.");
            }
        }

        private void SCWisp(CommandArgs args)
        {
            args.Player.SetBuff(57);
            args.Player.SendSuccessMessage("You spawned a Wisp! Right-click the wisp icon under your toolbar to deactivate.");
        }

        private void SCRudolph(CommandArgs args)
        {
            args.Player.SetBuff(90);
            args.Player.SendSuccessMessage("You spawned a Rudolph mount! Right-click the rudolph icon under your toolbar to deactivate.");
        }

        private void SCBunnyMount(CommandArgs args)
        {
            args.Player.SetBuff(128);
            args.Player.SendSuccessMessage("You spawned a Bunny mount! Right-click the bunny mount icon under your toolbar to deactivate.");
        }

        private void SCSlimeMount(CommandArgs args)
        {
            args.Player.SetBuff(130);
            args.Player.SendSuccessMessage("You spawned a Slime mount! Right-click the slime mount icon under your toolbar to deactivate.");
        }

        private void SCBeeMount(CommandArgs args)
        {
            args.Player.SetBuff(132);
            args.Player.SendSuccessMessage("You spawned a Hornet mount! Right-click the hornet mount icon under your toolbar to deactivate.");
        }

        #endregion

        #region Buffs
        private void SCBuffs(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid Syntax! Use /selfbuff <buff name>! Use \"/selfbuff list\" for a list of all buffs.");
            }
            else if (args.Parameters.Count == 1)
            {
                string type = args.Parameters[0].ToLower();

                if (type == "list")
                {
                    args.Player.SendInfoMessage("List of buffs: shine, gills, obsidianskin (skin), invisibility (invis), waterwalking (water), gravity (grav), lifeforce (life), featherfall (fall), shadoworb (orb), fairy (green/red/blue)");
                }
                else if (type == "shine")
                {
                    if (Players[args.Player.Index].hasShine == true)
                        args.Player.SendSuccessMessage("You have turned off your Shine permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Shine! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasShine = !Players[args.Player.Index].hasShine;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "gills")
                {
                    if (Players[args.Player.Index].hasGills == true)
                        args.Player.SendSuccessMessage("You have turned off your Gills permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Gills! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasGills = !Players[args.Player.Index].hasGills;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "obsidianskin" || type == "skin")
                {
                    if (Players[args.Player.Index].hasSkin == true)
                        args.Player.SendSuccessMessage("You have turned off your Obsidian Skin permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Obsidian Skin! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasSkin = !Players[args.Player.Index].hasSkin;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "invisibility" || type == "invis")
                {
                    if (Players[args.Player.Index].hasInvis == true)
                        args.Player.SendSuccessMessage("You have turned off your Invisibility permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Invisibility! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasInvis = !Players[args.Player.Index].hasInvis;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "waterwalking" || type == "water")
                {
                    if (Players[args.Player.Index].hasWaterW == true)
                        args.Player.SendSuccessMessage("You have turned off your Water Walking permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Water Walking! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasWaterW = !Players[args.Player.Index].hasWaterW;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "gravity" || type == "grav")
                {
                    if (Players[args.Player.Index].hasGrav == true)
                        args.Player.SendSuccessMessage("You have turned off your Gravity permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Gravity! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasGrav = !Players[args.Player.Index].hasGrav;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "lifeforce" || type == "life")
                {
                    if (Players[args.Player.Index].hasLife == true)
                        args.Player.SendSuccessMessage("You have turned off your Lifeforce permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Lifeforce! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasLife = !Players[args.Player.Index].hasLife;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "featherfall" || type == "fall")
                {
                    if (Players[args.Player.Index].hasFall == true)
                        args.Player.SendSuccessMessage("You have turned off your Featherfall permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Featherfall! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasFall = !Players[args.Player.Index].hasFall;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (type == "shadoworb" || type == "orb")
                {
                    if (Players[args.Player.Index].hasOrb == true)
                        args.Player.SendSuccessMessage("You have turned off your Shadow Orb permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Shadow Orb! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasOrb = !Players[args.Player.Index].hasOrb;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else
                    args.Player.SendErrorMessage("Invalid buff name! Use /selfbuff <buff name>! Use \"/selfbuff list\" for a list of all buffs.");
            }
            else if (args.Parameters.Count == 2)
            {
                if (args.Parameters[0].ToLower() == "obsidian" && args.Parameters[1].ToLower() == "skin")
                {
                    if (Players[args.Player.Index].hasSkin == true)
                        args.Player.SendSuccessMessage("You have turned off your Obsidian Skin permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Obsidian Skin! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasSkin = !Players[args.Player.Index].hasSkin;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (args.Parameters[0].ToLower() == "water" && args.Parameters[1].ToLower() == "walking")
                {
                    if (Players[args.Player.Index].hasWaterW == true)
                        args.Player.SendSuccessMessage("You have turned off your Water Walking permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Water Walking! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasWaterW = !Players[args.Player.Index].hasWaterW;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (args.Parameters[0].ToLower() == "life" && args.Parameters[1].ToLower() == "force")
                {
                    if (Players[args.Player.Index].hasLife == true)
                        args.Player.SendSuccessMessage("You have turned off your Lifeforce permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Lifeforce! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasLife = !Players[args.Player.Index].hasLife;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (args.Parameters[0].ToLower() == "feather" && args.Parameters[1].ToLower() == "fall")
                {
                    if (Players[args.Player.Index].hasFall == true)
                        args.Player.SendSuccessMessage("You have turned off your Featherfall permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Featherfall! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasFall = !Players[args.Player.Index].hasFall;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else if (args.Parameters[0].ToLower() == "shadow" && args.Parameters[1].ToLower() == "orb")
                {
                    if (Players[args.Player.Index].hasOrb == true)
                        args.Player.SendSuccessMessage("You have turned off your Shadow Orb permabuff. Right-click the buff icon to remove immediately.");
                    else
                        args.Player.SendSuccessMessage("You have permabuffed yourself with Shadow Orb! Type the command again to turn off the permabuff.");

                    Players[args.Player.Index].hasOrb = !Players[args.Player.Index].hasOrb;
                    Players[args.Player.Index].needsUpdating = true;
                }
                else
                {
                    args.Player.SendErrorMessage("Invalid buff name! Use /selfbuff <buff name>! Use \"/selfbuff list\" for a list of all buffs.");
                }
            }
            else
            {
                args.Player.SendErrorMessage("Invalid Syntax! Use /selfbuff <buff name>! Use \"/selfbuff list\" for a list of all buffs.");
            }
        }
        #endregion

        #region npcs
        private void SCSM(CommandArgs args)
        {
            DateTime timenow = DateTime.Now;
            TimeSpan diff = new TimeSpan(timenow.Ticks - Players[args.Player.Index].lastSM.Ticks);
            TimeSpan minute = new TimeSpan(0, 1, 0);
            if (diff > minute)
            {
                bool success = false;

                if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
                {
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /spawnmob <mob type> [amount]");
                    return;
                }
                if (args.Parameters[0].Length == 0)
                {
                    args.Player.SendErrorMessage("Invalid mob type!");
                    return;
                }

                int amount = 1;
                if (args.Parameters.Count == 2 && !int.TryParse(args.Parameters[1], out amount))
                {
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /spawnmob <mob type> [amount]");
                    return;
                }

                amount = Math.Min(amount, Main.maxNPCs);

                var npcs = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
                if (npcs.Count == 0)
                {
                    args.Player.SendErrorMessage("Invalid mob type!");
                }
                else if (npcs.Count > 1)
                {
                    TShock.Utils.SendMultipleMatchError(args.Player, npcs.Select(n => n.name));
                }
                else
                {
                    var npc = npcs[0];
                    if (npc.type >= 1 && npc.type < Main.maxNPCTypes && npc.type != 113)
                    {
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY, 50, 20);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned {1} {2} time(s).", args.Player.Name, npc.name, amount);
                        success = true;
                    }
                    else if (npc.type == 113)
                    {
                        if (Main.wof >= 0 || (args.Player.Y / 16f < (Main.maxTilesY - 205)))
                        {
                            args.Player.SendErrorMessage("Can't spawn Wall of Flesh!");
                            return;
                        }
                        NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
                        TSPlayer.All.SendSuccessMessage("{0} has spawned Wall of Flesh!", args.Player.Name);
                        success = true;
                    }
                    else
                    {
                        args.Player.SendErrorMessage("Invalid mob type!");
                    }
                }

                if (success)
                    Players[args.Player.Index].lastSM = DateTime.Now;
            }
            else
            {
                int elapsed;
                if (Players[args.Player.Index].lastSM.Second > timenow.Second)
                    elapsed = (timenow.Second + 60) - Players[args.Player.Index].lastSM.Second;
                else
                    elapsed = timenow.Second - Players[args.Player.Index].lastSM.Second;

                int ielapsed = 60 - elapsed;
                string selapsed = ielapsed.ToString();

                args.Player.SendErrorMessage("Please wait " + selapsed + " seconds to use this command!");
            }
        }

        private void SCSB(CommandArgs args)
        {
            DateTime timenow = DateTime.Now;
            TimeSpan diff = new TimeSpan(timenow.Ticks - Players[args.Player.Index].lastSB.Ticks);
            TimeSpan minute = new TimeSpan(0, 1, 0);
            if (diff > minute)
            {
                /*int count = 0;
                string mobs = "";
                while (count < args.Parameters.Count)
                {
                    mobs += args.Parameters[count];
                    count++;
                    if (count < args.Parameters.Count)
                        mobs += " ";
                }
                string usethis = "/spawnboss eye";
                if (mobs != "*")
                    usethis = "/spawnboss " + mobs;
                args.Player.SendInfoMessage("LastSBMinutes: " + Players[args.Player.Index].lastSB.Minute.ToString());
                args.Player.SendInfoMessage("TimeNowMinutes: " + timenow.Minute.ToString());
                Commands.HandleCommand(args.Player, usethis);

                Players[args.Player.Index].lastSB = DateTime.Now;*/

                //SpawnBoss TShock Code

                bool success = false;

                if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
                {
                    args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /boss <boss type> [amount]");
                    return;
                }

                int amount = 1;
                if (args.Parameters.Count == 2 && (!int.TryParse(args.Parameters[1], out amount) || amount <= 0))
                {
                    args.Player.SendErrorMessage("Invalid boss amount!");
                    return;
                }

                NPC npc = new NPC();
                switch (args.Parameters[0].ToLower())
                {
                    case "brain":
                    case "brain of cthulhu":
                        npc.SetDefaults(266);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned the Brain of Cthulhu {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "destroyer":
                        npc.SetDefaults(134);
                        TSPlayer.Server.SetTime(false, 0.0);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned the Destroyer {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "duke":
                    case "duke fishron":
                    case "fishron":
                        npc.SetDefaults(370);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned Duke Fishron {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "eater":
                    case "eater of worlds":
                        npc.SetDefaults(13);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned the Eater of Worlds {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "eye":
                    case "eye of cthulhu":
                        npc.SetDefaults(4);
                        TSPlayer.Server.SetTime(false, 0.0);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned the Eye of Cthulhu {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "golem":
                        npc.SetDefaults(245);
                        TSPlayer.Server.SetTime(false, 0.0);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned Golem {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "king":
                    case "king slime":
                        npc.SetDefaults(50);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned King Slime {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "plantera":
                        npc.SetDefaults(262);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned Plantera {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "prime":
                    case "skeletron prime":
                        npc.SetDefaults(127);
                        TSPlayer.Server.SetTime(false, 0.0);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned Skeletron Prime {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "queen":
                    case "queen bee":
                        npc.SetDefaults(222);
                        TSPlayer.Server.SetTime(false, 0.0);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned Queen Bee {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "skeletron":
                        npc.SetDefaults(35);
                        TSPlayer.Server.SetTime(false, 0.0);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned Skeletron {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "twins":
                        TSPlayer.Server.SetTime(false, 0.0);
                        npc.SetDefaults(125);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        npc.SetDefaults(126);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.name, amount, args.Player.TileX, args.Player.TileY);
                        TSPlayer.All.SendSuccessMessage("{0} has spawned the Twins {1} time(s).", args.Player.Name, amount);
                        success = true;
                        break;
                    case "wof":
                    case "wall of flesh":
                        if (Main.wof >= 0)
                        {
                            args.Player.SendErrorMessage("There is already a Wall of Flesh!");
                            return;
                        }
                        if (args.Player.Y / 16f < Main.maxTilesY - 205)
                        {
                            args.Player.SendErrorMessage("You must spawn the Wall of Flesh in hell!");
                            return;
                        }
                        NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
                        TSPlayer.All.SendSuccessMessage("{0} has spawned the Wall of Flesh.", args.Player.Name);
                        success = true;
                        break;
                    default:
                        args.Player.SendErrorMessage("Invalid boss type!");
                        break;
                }

                if (success)
                    Players[args.Player.Index].lastSB = DateTime.Now;
            }
            else
            {
                int elapsed;
                if (Players[args.Player.Index].lastSB.Second > timenow.Second)
                    elapsed = (timenow.Second + 60) - Players[args.Player.Index].lastSB.Second;
                else
                    elapsed = timenow.Second - Players[args.Player.Index].lastSB.Second;

                int ielapsed = 60 - elapsed;
                string selapsed = ielapsed.ToString();

                args.Player.SendErrorMessage("Please wait " + selapsed + " seconds to use this command!");
            }
        }

        private void SCBN(CommandArgs args)
        {
            DateTime timenow = DateTime.Now;
            TimeSpan diff = new TimeSpan(timenow.Ticks - Players[args.Player.Index].lastBN.Ticks);
            TimeSpan minute = new TimeSpan(0, 1, 0);
            if (diff > minute)
            {
                Commands.HandleCommand(args.Player, "/butchernear");
                Players[args.Player.Index].lastBN = DateTime.Now;
            }
            else
            {
                int elapsed;
                if (Players[args.Player.Index].lastBN.Second > timenow.Second)
                    elapsed = (timenow.Second + 60) - Players[args.Player.Index].lastBN.Second;
                else
                    elapsed = timenow.Second - Players[args.Player.Index].lastBN.Second;

                int ielapsed = 60 - elapsed;
                string selapsed = ielapsed.ToString();

                args.Player.SendErrorMessage("Please wait " + selapsed + " seconds to use this command!");
            }
        }
        #endregion

        #region UserxUser
        private void SCPoke(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /poke <player>");
                return;
            }
            if (args.Parameters[0].Length == 0)
            {
                args.Player.SendErrorMessage("Invalid player!");
                return;
            }

            string plStr = args.Parameters[0];
            var players = TShock.Utils.FindPlayer(plStr);
            if (players.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid player!");
            }
            else if (players.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
            }
            else
            {
                var plr = players[0];
                plr.DamagePlayer(1);
                args.Player.SendInfoMessage("You poked {0}.", plr.Name);
                TSPlayer.All.SendSuccessMessage("{0} poked {1}.", args.Player.Name, plr.Name);
                Log.Info("{0} poked {1}.", args.Player.Name, plr.Name);
            }
        }

        private void SCSuPoke(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /supoke <player>");
                return;
            }
            if (args.Parameters[0].Length == 0)
            {
                args.Player.SendErrorMessage("Invalid player!");
                return;
            }

            string plStr = args.Parameters[0];
            var players = TShock.Utils.FindPlayer(plStr);
            if (players.Count == 0)
            {
                args.Player.SendErrorMessage("Invalid player!");
            }
            else if (players.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
            }
            else
            {
                var plr = players[0];
                plr.DamagePlayer(30000);
                args.Player.SendInfoMessage("You poked {0}. BOOM!", plr.Name);
                TSPlayer.All.SendSuccessMessage("{0} poked {1}. BOOM!", args.Player.Name, plr.Name);
                Log.Info("{0} super-poked {1}.", args.Player.Name, plr.Name);
            }
        }

        private void SCHug(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /hug <player>");
                return;
            }
            if (args.Parameters[0].Length == 0)
            {
                args.Player.SendErrorMessage("Invalid player!");
                return;
            }

            string plStr = args.Parameters[0];
            var players = TShock.Utils.FindPlayer(plStr);
            if (players.Count == 0)
            {
                args.Player.SendInfoMessage("You hugged your invisible friend {0}!", plStr);
                TSPlayer.All.SendSuccessMessage("{0} hugged " + (args.Player.TPlayer.male ? "his" : "her") + " invisible friend {1}!", args.Player.Name, plStr);
            }
            else if (players.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
            }
            else
            {
                var plr = players[0];
                args.Player.SendInfoMessage("You hugged {0}!", plr.Name);
                TSPlayer.All.SendSuccessMessage("{0} hugged {1}!", args.Player.Name, plr.Name);
                Log.Info("{0} hugged {1}!", args.Player.Name, plr.Name);
            }
        }

        private void SCLick(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /lick <player>");
                return;
            }
            if (args.Parameters[0].Length == 0)
            {
                args.Player.SendErrorMessage("Invalid player!");
                return;
            }

            string plStr = args.Parameters[0];
            var players = TShock.Utils.FindPlayer(plStr);
            if (players.Count == 0)
            {
                args.Player.SendInfoMessage("You licked the air! {0} was not found...", plStr);
                TSPlayer.All.SendSuccessMessage("{0} licked the air!", args.Player.Name);
            }
            else if (players.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
            }
            else
            {
                var plr = players[0];
                args.Player.SendInfoMessage("You licked {0}!", plr.Name);
                TSPlayer.All.SendSuccessMessage("{0} licked {1}!", args.Player.Name, plr.Name);
                Log.Info("{0} licked {1}!", args.Player.Name, plr.Name);
            }
        }

        private void SCFPalm(CommandArgs args)
        {
            
                args.Player.SendInfoMessage("You facepalmed.");
                TSPlayer.All.SendSuccessMessage("{0} facepalmed.", args.Player.Name);
                Log.Info("{0} facepalmed.", args.Player.Name);
            
        }

        private void SCFBook(CommandArgs args)
        {
            args.Player.SendInfoMessage("You hit your face with the nearest, heaviest book.");
            TSPlayer.All.SendSuccessMessage("{0} hit " + (args.Player.TPlayer.male ? "his" : "her") + " face with the nearest, heaviest book.", args.Player.Name);
        }

        private void SCStab(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /stab <player>");
                return;
            }
            if (args.Parameters[0].Length == 0)
            {
                args.Player.SendErrorMessage("Invalid player!");
                return;
            }

            string plStr = args.Parameters[0];
            var players = TShock.Utils.FindPlayer(plStr);
            if (players.Count == 0)
            {
                args.Player.SendInfoMessage("You stabbed your invisible friend {0}!", plStr);
                TSPlayer.All.SendSuccessMessage("{0} stabbed " + (args.Player.TPlayer.male ? "his" : "her") + " invisible friend {1}!", args.Player.Name, plStr);
            }
            else if (players.Count > 1)
            {
                TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
            }
            else
            {
                var plr = players[0];
                plr.DamagePlayer(9001);
                args.Player.SendInfoMessage("You stabbed {0} for OVER 9000 damage!", plr.Name);
                TSPlayer.All.SendSuccessMessage("{0} stabbed {1} mercilessly!", args.Player.Name, plr.Name);
                Log.Info("{0} stabbed {1}.", args.Player.Name, plr.Name);
            }
        }

        private void SCFDesk(CommandArgs args)
        {
            args.Player.DamagePlayer(300);
            if (!args.Player.RealPlayer)
                args.Player.SendInfoMessage("You slammed your face on a desk.");
            TSPlayer.All.SendSuccessMessage("{0} slammed " + (args.Player.TPlayer.male ? "his" : "her") + " face on a desk.", args.Player.Name);
        }

        private void SCFPlant(CommandArgs args)
        {
            if (!args.Player.RealPlayer)
                args.Player.SendInfoMessage("You planted your face on the ground.");
            else
                args.Player.DamagePlayer(1000);
            TSPlayer.All.SendSuccessMessage("{0} planted " + (args.Player.TPlayer.male ? "his" : "her") + " face on the ground.", args.Player.Name);
        }

        private void SCFWall(CommandArgs args)
        {
            if (!args.Player.RealPlayer)
                args.Player.SendInfoMessage("You ran into a wall at very high speeds.");
            else
                args.Player.DamagePlayer(1000);
            TSPlayer.All.SendSuccessMessage("{0} ran into a wall at very high speeds.", args.Player.Name);
        }

        private void SCSlapAll(CommandArgs args)
        {
            if (!args.Player.RealPlayer)
                args.Player.SendInfoMessage("You slapped everyone! That stings!");
            TSPlayer.All.SendInfoMessage("{0} slapped you (along with everyone else)!", args.Player.Name);
            TSPlayer.All.DamagePlayer(15);
        }

        private void SCRape(CommandArgs args)
        {
            if (args.Parameters.Count != 1)
                args.Player.SendErrorMessage("Invalid syntax: /rape <player>");
            else
            {
                string plStr = args.Parameters[0];
                var players = TShock.Utils.FindPlayer(plStr);
                if (players.Count == 0)
                {
                    args.Player.SendErrorMessage("Player not found.");
                }
                else if (players.Count > 1)
                {
                    TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
                }
                else if (players[0].UserAccountName == "Patrikk")
                {
                    args.Player.SendErrorMessage("You cannot rape the master of rapes!", args.Player.Name, players[0].Name);
                    TSPlayer.All.SendSuccessMessage("{0} tried to rape {1}! {1} laughed and promptly banned {0}.", args.Player.Name, players[0].Name);
                    try
                    {
                        TShock.Bans.AddBan(args.Player.IP, args.Player.Name, args.Player.UUID, "You were banned for trying to rape your master.");
                        TShock.Bans.RemoveBan(args.Player.Name, true);
                    }
                    catch (Exception)
                    {
                        TShock.Bans.AddBan(args.Player.IP, args.Player.Name, "You were banned for trying to rape your master.");
                        TShock.Bans.RemoveBan(args.Player.IP);
                    }                    
                }
                else
                {
                    args.Player.SendSuccessMessage("You raped {0}. You feel slightly better.", players[0].Name);
                    players[0].SendInfoMessage("You were raped by {0}.", args.Player.Name);
                    TSPlayer.All.SendSuccessMessage("{0} raped {1}! Is that blood?", args.Player.Name, players[0].Name);
                    players[0].SetBuff(26, 7200); //Well Fed; 2 minutes
                    players[0].SetBuff(30, 21600); //Bleeding; 6 minutes
                    players[0].SetBuff(33, 7200); //Weak
                    players[0].SetBuff(46, 7200); //Chilled
                    players[0].SetBuff(63); // Panic!
                    players[0].SetBuff(103, 7200); //Wet
                    players[0].SetBuff(120, 7200); //Stinky
                }
            }
        }

        #endregion

        #region Ranks
        private void SCUser(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendErrorMessage("Invalid syntax: /upgrade <account>");
                return;
            }

            var str = string.Join(" ", args.Parameters);
            var player = TShock.Utils.FindPlayer(args.Parameters[0]);

            if (player.Count > 1)
                TShock.Utils.SendMultipleMatchError(args.Player, player.Select(p => p.Name));
            else if (player.Count < 1)
                args.Player.SendErrorMessage("Invalid player!");
            else
            {
                if (player[0].Group != TShock.Utils.GetGroup("default"))
                    args.Player.SendErrorMessage("You can only upgrade User accounts!");
                else
                {
                    TShock.Users.SetUserGroup(TShock.Users.GetUserByName(player[0].Name), "user+");
                    args.Player.SendSuccessMessage("Successfully upgraded {0} to group User+!", player[0].Name);
                }
            }
        }

        private void SCBuilder1(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendErrorMessage("Invalid syntax: /b1 <account>");
                return;
            }

            var str = string.Join(" ", args.Parameters);
            var player = TShock.Utils.FindPlayer(args.Parameters[0]);

            if (player.Count > 1)
                TShock.Utils.SendMultipleMatchError(args.Player, player.Select(p => p.Name));
            else if (player.Count < 1)
                args.Player.SendErrorMessage("Invalid player!");
            else
            {
                TShock.Users.SetUserGroup(TShock.Users.GetUserByName(player[0].Name), "builder1");
                args.Player.SendSuccessMessage("Successfully upgraded {0} to group builder1!", player[0].Name);
            }
        }

        private void SCBuilder2(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendErrorMessage("Invalid syntax: /b1 <account>");
                return;
            }

            var str = string.Join(" ", args.Parameters);
            var player = TShock.Utils.FindPlayer(args.Parameters[0]);

            if (player.Count > 1)
                TShock.Utils.SendMultipleMatchError(args.Player, player.Select(p => p.Name));
            else if (player.Count < 1)
                args.Player.SendErrorMessage("Invalid player!");
            else
            {
                if (player[0].Group != TShock.Utils.GetGroup("builder1"))
                    args.Player.SendErrorMessage("You can only promote Builder1 accounts!");
                else
                {
                    TShock.Users.SetUserGroup(TShock.Users.GetUserByName(player[0].Name), "builder2");
                    args.Player.SendSuccessMessage("Successfully upgraded {0} to group builder2!", player[0].Name);
                }
            }
        }

        #endregion

        private void SCWebsite(CommandArgs args)
        {
            args.Player.SendInfoMessage("The website for Aurora Terraria is http://www.aurora-terraria.org/");
        }

        private void SCInfo(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
                args.Player.SendErrorMessage("Invalid syntax! Use /ranks <rank name> to view information about a rank, or /ranks list for a list of ranks.");
            else if (args.Parameters.Count == 1)
            {
                string rank = args.Parameters[0].ToLower();

                if (rank == "list")
                    args.Player.SendInfoMessage("List of ranks: guest, user, user+, griefwatch, moderator, administrator, builder1, builder2, builder3, donator1, donator2, donator3, donator4, vip, manager, owner");
                else if (rank == "guest" || rank == "register" || rank == "register!")
                    args.Player.SendMessage("The [Register!] rank (aka guest) is rank you receive when you are not logged-in. Abilities are limited to moving around, chatting, and PvP.", 100, 100, 100);
                else if (rank == "user+")
                    args.Player.SendMessage("The [User+] rank is earned by making an introduction thread on the forum. This rank allows you to paint, use the /msg command, /poke, and various other commands.", 153, 153, 255);
                else if (rank == "user")
                    args.Player.SendMessage("The [User] rank is the rank you receive when you register on the server. This rank allows you to build, teleport, warp, whisper, and more.", Color.White);
                else if (rank == "gw" || rank == "griefwatch")
                    args.Player.SendMessage("The [Grief Watch] rank is earned by being active and helpful on the server. If you feel you would be able to take care of the server for a while, contact a staff member and they will discuss it with you.", 239, 146, 180);
                else if (rank == "mod" || rank == "moderator")
                    args.Player.SendMessage("The [Moderator] rank is earned by applying on the forum. This ranks allows you to kick/ban players, spawn & butcher mobs, use banned items, and various other commands.", 67, 157, 247);
                else if (rank == "admin" || rank == "administrator")
                    args.Player.SendMessage("The [Administrator] rank is earned by proving yourself as a great and helpful moderator. This rank allows you to use WorldEdit, change nicknames, start invasions, and more.", 233, 55, 49);
                else if (rank == "builder")
                    args.Player.SendErrorMessage("Use /ranks builder1, /ranks builder2, or /ranks builder3.");
                else if (rank == "builder1" || rank == "b1")
                    args.Player.SendMessage("The [Builder][1] rank is earned by applying on the forum. This rank allows you to set 5 homes, use the Rod of Discord, give yourself buffs, and some additional commands.", 55, 235, 139);
                else if (rank == "builder2" || rank == "b2")
                    args.Player.SendMessage("The [Builder][2] rank is earned by applying on the forum. This rank allows you to use banned items, protect buildings, set the time, and various additional commands.", 124, 223, 37);
                else if (rank == "builder3" || rank == "b3")
                    args.Player.SendMessage("The [Builder][3] rank is earned by applying on the forum. This rank allows you to use WorldEdit, rollback griefed buildings, and some additional commands.", 16, 223, 99);
                else if (rank == "donator")
                    args.Player.SendErrorMessage("Use /ranks donator1, /ranks donator2, /ranks donator3, or /ranks donator4.");
                else if (rank == "donator1" || rank == "d1")
                    args.Player.SendMessage("The [Donator][1] rank is earned by donating at least $5 to the server. This rank allows you nothing, you cheap bastard.", 193, 107, 250);
                else if (rank == "donator2" || rank == "d2")
                    args.Player.SendMessage("The [Donator][2] rank is earned by donating at least $10 to the server. This rank allows you to set 8 homes, buff yourself, use pet commands, and various other commands.", 171, 80, 240);
                else if (rank == "donator3" || rank == "d3")
                    args.Player.SendMessage("The [Donator][3] rank is earned by donating at least $20 to the server. This rank allows you to set 12 homes, use the Rod of Discord, spawn mobs, and various other commands.", 149, 58, 219);
                else if (rank == "donator4" || rank == "d4")
                    args.Player.SendMessage("The [Donator][4] rank is earned by donating at least $50 to the server. This rank allows you to set unlimited homes, use /icewand, spawn & butcher mobs, and various other commands.", 134, 45, 202);
                else if (rank == "vip")
                    args.Player.SendMessage("The [VIP] rank is given to people chosen by the owner, usually personal friends, long-time and/or retired staff, or guests from the Terraria dev team.", 234, 217, 61);
                else if (rank == "manager")
                    args.Player.SendMessage("The [Manager] rank is reserved for the server manager. The manager is in charge of the website, forum, server databases, and other server-related jobs.", 147, 112, 219);
                else if (rank == "owner" || rank == "superadmin")
                    args.Player.SendMessage("The [Owner] rank is reserverd for the server owner. Duh. You get this rank by hax.", 81, 215, 236);
                else
                    args.Player.SendErrorMessage("Unknown rank. Use /ranks list for a list of included ranks.");
            }
            else if (args.Parameters.Count == 2)
            {
                string rank = args.Parameters[0].ToLower() + " " + args.Parameters[1].ToLower();
                if (rank == "grief watch")
                    args.Player.SendMessage("The [Grief Watch] rank is earned by being active and helpful on the server. If you feel you would be able to take care of the server for a while, contact a staff member and they will discuss it with you.", 239, 146, 180);
                else if (rank == "builder 1" || rank == "builder one")
                    args.Player.SendMessage("The [Builder][1] rank is earned by applying on the forum. This rank allows you to set 5 homes, use the Rod of Discord, give yourself buffs, and some additional commands.", 55, 235, 139);
                else if (rank == "builder 2" || rank == "builder two")
                    args.Player.SendMessage("The [Builder][2] rank is earned by applying on the forum. This rank allows you to use banned items, protect buildings, set the time, and various additional commands.", 124, 223, 37);
                else if (rank == "builder 3" || rank == "builder three")
                    args.Player.SendMessage("The [Builder][3] rank is earned by applying on the forum. This rank allows you to use WorldEdit, rollback griefed buildings, and some additional commands.", 16, 223, 99);
                else if (rank == "donator 1" || rank == "donator one")
                    args.Player.SendMessage("The [Donator][1] rank is earned by donating at least $5 to the server. This rank allows you nothing, you cheap bastard.", 193, 107, 250);
                else if (rank == "donator 2" || rank == "donator two")
                    args.Player.SendMessage("The [Donator][2] rank is earned by donating at least $10 to the server. This rank allows you to set 8 homes, buff yourself, use pet commands, and various other commands.", 171, 80, 240);
                else if (rank == "donator 3" || rank == "donator three")
                    args.Player.SendMessage("The [Donator][3] rank is earned by donating at least $20 to the server. This rank allows you to set 12 homes, use the Rod of Discord, spawn mobs, and various other commands.", 149, 58, 219);
                else if (rank == "donator 4" || rank == "donator four")
                    args.Player.SendMessage("The [Donator][4] rank is earned by donating at least $50 to the server. This rank allows you to set unlimited homes, use /icewand, spawn & butcher mobs, and various other commands.", 134, 45, 202);
                else
                    args.Player.SendErrorMessage("Unknown rank. Use /ranks list for a list of included ranks.");
            }
            else
                args.Player.SendErrorMessage("Unknown rank. Use /ranks list for a list of included ranks.");
        }
    }
}
