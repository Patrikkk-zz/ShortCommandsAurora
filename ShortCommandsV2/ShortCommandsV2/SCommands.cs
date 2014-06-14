﻿using System;
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
        public override Version Version { get { return new Version("5.0"); } }
        
        public SCommands(Main game)
			: base(game)
		{
			base.Order = 1;
		}

        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreet);
            //ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnGreet);
                //ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
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
            Commands.ChatCommands.Add(new Command("history.get", SCClear, "ci") { HelpText = "ShortCommand for /clear item 30000" });
            Commands.ChatCommands.Add(new Command("worldedit.selection.point", SCPoint1, "p1") { AllowServer = false, HelpText = "ShortCommand for //point 1" });
            Commands.ChatCommands.Add(new Command("worldedit.selection.point", SCPoint2, "p2") { AllowServer = false, HelpText = "ShortCommand for //point 2" });
            Commands.ChatCommands.Add(new Command(SCList, "shortcommands") { AllowServer = false, HelpText = "Lists all ShortCommands available to you." });
            Commands.ChatCommands.Add(new Command("sc.pets", SCBunny, "bunny") { AllowServer = false, HelpText = "Spawns a Bunny pet!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCPenguin, "peng", "penguin") { AllowServer = false, HelpText = "Spawns a Penguin pet!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCTruffle, "truffle") { AllowServer = false, HelpText = "Spawns a baby Truffle pet!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCWisp, "wisp") { AllowServer = false, HelpText = "Spawns a Wisp!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCRudolph, "rudolph") { AllowServer = false, HelpText = "Spawns a Rudolph mount!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCBunnyMount, "bunnymount") { AllowServer = false, HelpText = "Spawns a Bunny mount!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCSlimeMount, "slimemount") { AllowServer = false, HelpText = "Spawns a Slime mount!" });
            Commands.ChatCommands.Add(new Command("sc.pets", SCBeeMount, "beemount") { AllowServer = false, HelpText = "Spawns a Hornet mount!" });
            Commands.ChatCommands.Add(new Command("sc.poke", SCPoke, "poke") { HelpText = "Pokes a player!" });
            Commands.ChatCommands.Add(new Command("sc.supoke", SCSuPoke, "supoke") { HelpText = "Super pokes a player." });
            Commands.ChatCommands.Add(new Command("sc.stab", SCStab, "stab") { HelpText = "Stabs a player!" });
            Commands.ChatCommands.Add(new Command("sc.hug", SCHug, "hug") { HelpText = "Hugs a player!" });
            Commands.ChatCommands.Add(new Command("sc.face", SCFPalm, "facepalm") { HelpText = "Performs a facepalm." });
            Commands.ChatCommands.Add(new Command("sc.face", SCFDesk, "facedesk") { HelpText = "Slams your face on a desk." });
            Commands.ChatCommands.Add(new Command("sc.face", SCFPlant, "faceplant") { HelpText = "Slams your face on a plant. Err... makes you faceplant." });
            Commands.ChatCommands.Add(new Command("sc.face", SCFWall, "facewall") { HelpText = "You run into a wall at high speeds!" });
            Commands.ChatCommands.Add(new Command("sc.face", SCFBook, "facebook") { HelpText = "Checks your friends' status messages." });
            Commands.ChatCommands.Add(new Command("sc.slapall", SCSlapAll, "slapall") { HelpText = "Slaps ALL the people!" });
            Commands.ChatCommands.Add(new Command("sc.rape", SCRape, "rape") { HelpText = "Rapes the given player." });
            Commands.ChatCommands.Add(new Command("sc.ranks", SCUser, "upgrade") { HelpText = "Adds user to User+ group." });
            Commands.ChatCommands.Add(new Command("sc.ranks", SCBuilder1, "b1") { HelpText = "Adds user to Builder 1 group." });
            Commands.ChatCommands.Add(new Command("sc.ranks", SCBuilder1, "b2") { HelpText = "Adds user to Builder 2 group." });
            Commands.ChatCommands.Add(new Command(SCWebsite, "website") { HelpText = "The website for Aurora Terraria." });
            Commands.ChatCommands.Add(new Command("tshock.world.modify", SCInfo, "ranks") { HelpText = "Information on how to get ranks.", AllowServer = false });
            Commands.ChatCommands.Add(new Command(SCCheck, "check") { HelpText = "Checks to see if the specified username is taken." });
        }

        public void OnGreet(GreetPlayerEventArgs args)
        {
            Commands.HandleCommand(TShock.Players[args.Who], "/check start");
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
        private void SCGod(CommandArgs args)
        {
            if (args.Parameters.Count == 0)
                Commands.HandleCommand(args.Player, "/godmode");
            else if (args.Parameters.Count == 1)
                Commands.HandleCommand(args.Player, "/godmode " + args.Parameters[0]);
            else if (args.Parameters.Count == 2)
                Commands.HandleCommand(args.Player, "/godmode " + args.Parameters[1]);
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
                else if (players[0].Group == TShock.Utils.GetGroup("admin") || players[0].Group == TShock.Utils.GetGroup("superadmin") || players[0].Group == TShock.Utils.GetGroup("moderator") || players[0].Group == TShock.Utils.GetGroup("L"))
                {
                    args.Player.SendErrorMessage("{0} used Rape! {1} avoided the attack!", args.Player.Name, players[0].Name);
                }
                else
                {
                    args.Player.SendSuccessMessage("You raped {0}. You feel slightly better.", players[0].Name);
                    players[0].SendInfoMessage("You were raped by {0}.", args.Player.Name);
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

        private void SCWebsite(CommandArgs args)
        {
            args.Player.SendInfoMessage("The website for Aurora Terraria is http://www.aurora-terraria.org/");
        }

        private void SCInfo(CommandArgs args)
        {
            args.Player.SendInfoMessage("To get [User+], go to http://www.aurora-terraria.org/forum/ and make an Introduction thread.");
            args.Player.SendInfoMessage("To get [Grief Watch], you will need to be active on this server and help out when possible.");
            args.Player.SendInfoMessage("To get [Builder][1], build at least 3 buildings, then go to http://www.aurora-terraria.org/forum/ and make a Builder application.");
            args.Player.SendInfoMessage("To get [Donator][1], you need to donate money to the server at http://www.aurora-terraria.org/donate/.");
        }
    }
}
