using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2015; 

internal class Day22 : PuzzleBase<Day22> {

    record Spell(
        string Name,
        int manacost,
        int damage,
        int heal,
        Effect? applyEffect
    );

    public class Effect {
        // Public properties
        public string Name { get; set; }
        public int Armor { get; set; }
        public int Damage { get; set; }
        public int Mana { get; set; }
        public int Duration { get; set; }

        // Constructor
        public Effect(string name, int armor, int damage, int mana, int duration) {
            Name = name;
            Armor = armor;
            Damage = damage;
            Mana = mana;
            Duration = duration;
        }
    }

    List<Spell> spells = [
        new("Magic Missile", 53, 4, 0, null),
        new("Drain", 73, 2, 2, null),
        new("Shield", 113, 0,0, new("Shield", 7,0,0,6)),
        new("Poison", 173, 0,0, new("Poison", 0,3,0,6)),
        new("Recharge", 229, 0,0, new("Recharge", 0,0,101,5)),
    ];


    class Player {
        public int hp { get; set; }
        public int mana { get; set; }
    }
    class Boss {
        public int hp { get; set; }
        public int dmg { get; set; }
    }

    class gameState {
        public Player player { get; set; }
        public Boss boss { get; set; }
        public List<Effect> Effects { get; set; }
        public int turn { get; set; }
        public int spentMana { get; set; }
        public bool end => player.hp <= 0 || boss.hp <= 0;
        public bool victory => player.hp > 0 && boss.hp <= 0;

        public gameState() { }

        public gameState(gameState inputState) {
            player = new Player() { hp = inputState.player.hp, mana = inputState.player.mana };
            boss = new Boss() { hp = inputState.boss.hp, dmg = inputState.boss.dmg };

            Effects = inputState.Effects.Select(e => new Effect(e.Name, e.Armor, e.Damage, e.Mana, e.Duration)).ToList();

            turn = inputState.turn;
            spentMana = inputState.spentMana;
        }
    };

    int SimulateWholeFight(Player player, Boss boss, int initialDmg = 0) {
        // get starting 
        var allGames = new List<gameState>();

        // set up initial gameState
        var initialState = new gameState() {
            player = player,
            boss = boss,
            Effects = new List<Effect>(),
            turn = 0,
            spentMana = 0
        };


        // Todo simulate all possibilitys until state.end and then save the spentMana into the list    
        GameLoop(initialState);

        var victorys = allGames.Where(x => x.victory).ToList();
        return victorys.Select(x=>x.spentMana).Min();

        void GameLoop(gameState input) {
            var state = new gameState(input);
            var possibleSpells = possibleSpellsForThisRound(state).ToList();

            // instantLoose
            if(possibleSpells.Count == 0) { return; }

            foreach(var item in possibleSpells) {
                var newGameState = new gameState(state);
                newGameState.turn++;
                newGameState = SimulateOneRound(item, newGameState);

                //Check if end is set
                if(newGameState.end) {
                    allGames.Add(newGameState);
                    continue;
                }else {
                    GameLoop(newGameState);
                }             
            }
        }

        // Get all possible spells for this round of game state
        List<Spell> possibleSpellsForThisRound(gameState currentState) => spells.Where(x => {
            if(currentState.player.mana < x.manacost)
                return false;

            // Allow a spell to be cast if it’s not active, or it’s an effect that should persist.
            if(currentState.Effects.Any(y => y.Name == x.Name && y.Duration > 1))
                return false;

            return true;
        }).ToList();

        // Inner function simulate 1 return new gamestate
        gameState SimulateOneRound(Spell spell, gameState state) {
            // part2 hard mode

            state.player.hp -= initialDmg;
            if(state.player.hp <= 0) {
                return state;
            }


            // Apply effects at the start of the player's turn
            ApplyEffects(state);

            // Check if the boss is defeated after applying effects
            if(state.boss.hp <= 0) {
                return state;
            }

            // Keep track of mana spent
            state.spentMana += spell.manacost;
            state.player.mana -= spell.manacost;
            state.player.hp += spell.heal;
            state.boss.hp -= spell.damage;

            if(spell.applyEffect is not null)
                state.Effects.Add(new Effect(spell.applyEffect.Name, spell.applyEffect.Armor, spell.applyEffect.Damage, spell.applyEffect.Mana, spell.applyEffect.Duration));

            // Check if the boss is defeated after the player's turn
            if(state.boss.hp <= 0) {
                return state;
            }

            // Boss's turn
            ApplyEffects(state);

            // Check if the boss is defeated after applying effects
            if(state.boss.hp <= 0) {
                return state;
            }

            // Compute player's armor
            int playerArmor = state.Effects.Where(e => e.Armor > 0).Sum(e => e.Armor);

            var dmgToPlayer = state.boss.dmg - playerArmor;
            state.player.hp -= Math.Max(dmgToPlayer, 1);

            return state;

            void ApplyEffects(gameState state) {
                foreach(var effect in state.Effects.ToList()) {
                    // If duration is greater than zero, apply the effect
                    if(effect.Duration > 0) {
                        if(effect.Damage != 0) {
                            state.boss.hp -= effect.Damage;
                        }
                        if(effect.Mana != 0) {
                            state.player.mana += effect.Mana;
                        }
                        // Armor is handled during the boss's attack
                    } else {
                        // Remove expired effects
                        state.Effects.Remove(effect);
                    } 
                    // Decrease duration
                    effect.Duration--;
                }
            }
        }
    }


    public override string SolvePart1() {
        // cant be bothered to parse this from my input file
        var bossHp = 71;
        var bossDmg = 10;

        var player = new Player() { hp = 50, mana= 500 };
        var boss = new Boss() { hp = bossHp, dmg = bossDmg };

        var result = SimulateWholeFight(player, boss);

        return result.ToString();
    }

    public override string SolvePart2() {
        // cant be bothered to parse this from my input file
        var bossHp = 71;
        var bossDmg = 10;

        var player = new Player() { hp = 50, mana= 500 };
        var boss = new Boss() { hp = bossHp, dmg = bossDmg };

        var result = SimulateWholeFight(player, boss, 1);

        return result.ToString();
    }
}
