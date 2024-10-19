using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;
using Mono.Security.Authenticode;
using UnityEngine;


namespace LightBringers_Tribe
{
	public class LightBringersTribe : WildfrostMod
	{
		// Mod Constructor
		public LightBringersTribe(string modDirectory) : base(modDirectory)
		{
			Instance = this;
		}

		public override string GUID => "lordloko505.wildfrost.LightBringers";
		public override string[] Depends => new string[] { };
		public override string Title => "Light Bringers";
		public override string Description => "W.I.P";

		public static LightBringersTribe Instance;

		public static List<object> assets = new List<object>();

		private bool preLoaded = false;

		private void CreateModAssets()
		{
			assets.Add(TribeCopy("Basic","Draw")
				.WithFlag("Images/LightBringersFlag.png")
				
				
				
				
				
				
				);
			//Status Effects
			//Status 0: Summon Graft Grunt
			assets.Add(
				StatusCopy("Summon Fallow", "Summon Graft Grunt")
				.SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
				{
					((StatusEffectSummon)data).summonCard = TryGet<CardData>("graftGrunt");

				})
			);
			Debug.Log("[Lightbringers] Summon Graft Grunt Added"); //debug

			//Status 1: is an instant effect, effects placed immediately and then removed
			assets.Add(
				StatusCopy("Instant Summon Fallow", "Instant Summon Graft Grunt") //Changes Instant Summon Fallow > Instant Summon Graft Grunt
				.SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
				{
					((StatusEffectInstantSummon)data).targetSummon = TryGet<StatusEffectSummon>("Summon Graft Grunt");
				})
			);
			Debug.Log("[Lightbringers] Instant Summon Graft Grunt Added");

			//Status 2: Summon Graft Grunt On Deploy
			//Is an StatusEffectApplyXWhenDeployed
			assets.Add(
				StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Graft Grunt")
				.WithText("When deployed, summon {0}")
				.WithTextInsert("<card=lordloko505.wildfrost.LightbringersTribe.graftGrunt>")
				.SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
				{
					((StatusEffectApplyXWhenDeployed)data).effectToApply = TryGet<StatusEffectData>("Instant Summon Graft Grunt");
				})
			);
			Debug.Log("[LightBringers] Summon Graft Grunt When Deployed Added"); //debug

			//Status 3: On Turn Apply Block To Ally Behind
			assets.Add(
				StatusCopy("On Turn Apply Spice To AllyBehind", "On Turn Apply Block To AllyBehind")
				.WithText("Apply {0} to ally behind")
				.WithTextInsert("<keyword=block>")
				.SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
				{
					((StatusEffectApplyXOnTurn)data).effectToApply = TryGet<StatusEffectData>("Block");
				})
			);
			Debug.Log("[LightBringers] On Turn Apply Block To Ally Behind Added");

			//Status 4: Apply Scrap To Random Ally
			assets.Add(
				StatusCopy("When Hit Apply Block To RandomAlly", "When Hit Apply Scrap To RandomAlly")
				.WithText("When hit apply {0} to random ally")
				.WithTextInsert("<keyword=scrap>")
				.SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
				{
					((StatusEffectApplyXWhenHit)data).effectToApply = TryGet<StatusEffectData>("Instant Add Scrap");

				})
			);
			Debug.Log("[LightBringers] On Hit Apply Scrap To RandomAlly Added");

            //Status 5: Summon Tumble
            assets.Add(
                StatusCopy("Summon Fallow", "tumble")
                .SubscribeToAfterAllBuildEvent(delegate (StatusEffectData data)
                {
                    ((StatusEffectSummon)data).summonCard = TryGet<CardData>("tumble");

                })
            );
            Debug.Log("[Lightbringers] Summon Tumble"); //debug

            //Card Code


            //Card 0: RadicalTerrance
            assets.Add(
				new CardDataBuilder(this).CreateUnit("radicalTerrance", "Radical Terrance.")
				.SetSprites("RadicalTerrance.png", "RadicalTerrance BG.png")
				.SetStats(1, 99, 1)
				.WithCardType("BossSmall")
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("On Turn Escape To Self", 1)
					};
				})

		   );

		   //Card 1: Minn
		   assets.Add(
			   new CardDataBuilder(this).CreateUnit("minn", "Minn")
			   .SetSprites("Minn.png", "Minn BG.png")
			   .SetStats(5, 4, 0)
			   .WithCardType("Friendly")
			   .WithText("Triggers when an ally is healed")
			   .SubscribeToAfterAllBuildEvent(delegate (CardData data)
			   {
				   data.startWithEffects = new CardData.StatusEffectStacks[]
				   {
					   SStack("When Ally Is Healed Trigger To Self", 1)
				   };

			   })

		   );

			//Card 2:Alphi & GraftGrunt
			assets.Add(
				new CardDataBuilder(this).CreateUnit("alphi", "Alphi")
				.SetSprites("Alphi.png", "Alphi BG.png")
				.SetStats(8, 4, 4)
				.WithCardType("Friendly")
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("When Deployed Summon Graft Grunt", 1)
					};
				})
			);

			assets.Add(
				new CardDataBuilder(this).CreateUnit("graftGrunt", "Graft Grunt")
				.SetSprites("GraftGrunt.png","GraftGrunt BG.png")
				.SetStats(3,2,2)
				.WithCardType("Summoned")
				.WithFlavour("Ima Gonna Getcha!")
			);

			//Card 3: John Buffm'n
			assets.Add(
				new CardDataBuilder(this).CreateUnit("johnBuff", "John Buffm'n")
				.SetSprites("JohnBuffm'n.png", "JohnBuffm'n BG.png")
				.SetStats(9, 3, 4)
				.WithCardType("Friendly")
				.SetTraits(TStack("Knockback",1))
			);

			//Card 4: Novus (Nova)
			assets.Add(
				new CardDataBuilder(this).CreateUnit("novus", "Novus")
				.SetSprites("Nova.png", "Nova BG.png")
				.SetStats(7, 0, 5)
				.WithCardType("Friendly")
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("On Turn Apply Block To AllyBehind", 1)
					};
				})
			);
			//Card 5: Pike
			assets.Add(
				new CardDataBuilder(this).CreateUnit("pike", "Pike")
				.SetSprites("Pike.png", "Pike BG.png")
				.SetStats(7, 2, 4)
				.WithCardType("Friendly")
				.SetTraits(TStack("Barrage",1))
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("When Hit Apply Scrap To RandomAlly", 1)
					};
				})
			);
			//Card 6: Lil'Squire
			assets.Add(
				new CardDataBuilder(this).CreateUnit("littleSquire", "Lil'Squire")
				.SetSprites("Lil'Squire.png", "Lil'Squire BG.png")
				.SetStats(null, 3, 0)
				.WithCardType("Clunker")
				.SetTraits(TStack("Smackback", 1))
                .SubscribeToAfterAllBuildEvent(delegate (CardData data)
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 2)
                    };
                })
            );

			//Card 7: Rough & Tumble
			assets.Add(
				new CardDataBuilder(this).CreateUnit("rough", "Rough")
				.SetSprites("Rough.png", "Rough BG.png")
				.SetStats(4, 1, 2)
				.WithCardType("Friendly")
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Teeth",2),
						SStack("On Turn Apply Teeth To Self",1)
					};
				})
			);

			assets.Add(
				new CardDataBuilder(this).CreateUnit("tumble", "Tumble")
				.SetSprites("Tumble.png","Tumble.png")
				.SetStats(5,1,2)
				.WithCardType("Summoned")
				.SetTraits(TStack("Frontline",1))
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Teeth",1)
					};
				})
			);



			//Card 8: Tank
			assets.Add(
				new CardDataBuilder(this).CreateUnit("tank", "Tank")
				.SetSprites("Tank.png", "Tank BG.png")
				.SetStats(10, 4, 8)
				.WithCardType("Friendly")
				.SetTraits(TStack("Bombard 1", 1))
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("When Hit Reduce Counter To Self", 1)
					};
				})
			);

			//Card 9: The Boy
			assets.Add(
				new CardDataBuilder(this).CreateUnit("theBoy", "The Boy")
				.SetSprites("TheBoy.png", "TheBoy BG.png")
				.SetStats(8, 1, 4)
				.WithCardType("Friendly")
				.SubscribeToAfterAllBuildEvent(delegate (CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("When Hit Apply Spice To Self",3)
					};
				})
			);

            //Card 10: Pullley
            assets.Add(
				new CardDataBuilder(this).CreateUnit("pulley", "Pulley")
				.SetSprites("Pulley.png", "Pulley BG.png")
				.SetStats(3, 2, 3)
				.IsPet("yes")
				.WithCardType("Friendly")
				.SetTraits(TStack("Aimless", 1), TStack("Pull", 1))
				.SubscribeToAfterAllBuildEvent(delegate(CardData data)
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("On Hit Equal Overload To Target",1)
					};
				})
			);

			//Card 11: Tangy
			assets.Add(
				new CardDataBuilder(this).CreateUnit("tangy", "Tangy")
				.SetSprites("Tangy.png","Tangy BG.png")
				.SetStats(5,2,3)
			);

			preLoaded = true;
		}

		protected override void Load()
		{
			if (!preLoaded) { CreateModAssets(); } //Builders arent made again upon 2nd load
			base.Load();
		}

		protected override void Unload()
		{
			base.Unload();
		}

		public override List<T> AddAssets<T, Y>() //inside base.Load(), called mutltiple times, each time T+Y are different DataFile and DataFileBuilders
		{
			if (assets.OfType<T>().Any())
				Debug.LogWarning($"[{Title}] adding {typeof(Y).Name}s: {assets.OfType<T>().Count()}"); //Debug statment
			return assets.OfType<T>().ToList();
		}

		public T TryGet<T>(string name) where T : DataFile
		{
			T data;
			if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
				data = base.Get<StatusEffectData>(name) as T;
			else
				data = base.Get<T>(name);

			if (data == null)
				throw new Exception($"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{name}] or [{Extensions.PrefixGUID(name, this)}]");

			return data;
		}

		public CardData.StatusEffectStacks SStack(string name, int amount) => new CardData.StatusEffectStacks(TryGet<StatusEffectData>(name), amount);
        public CardData.TraitStacks TStack(string name, int amount) => new CardData.TraitStacks(TryGet<TraitData>(name), amount);
        public StatusEffectDataBuilder StatusCopy(string oldName, string newName)
		{
			StatusEffectData data = TryGet<StatusEffectData>(oldName).InstantiateKeepName();
			data.name = GUID + "." + newName;
			data.targetConstraints = new TargetConstraint[0];
			StatusEffectDataBuilder builder = data.Edit<StatusEffectData, StatusEffectDataBuilder>();
			builder.Mod = this;
			return builder;
		}

        private CardDataBuilder CardCopy(string oldName, string newName)
        {
            CardData data = TryGet<CardData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            CardDataBuilder builder = data.Edit<CardData, CardDataBuilder>();
            builder.Mod = this;
            return builder;
        }

        private ClassDataBuilder TribeCopy(string oldName, string newName)
        {
            ClassData data = TryGet<ClassData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            ClassDataBuilder builder = data.Edit<ClassData, ClassDataBuilder>();
            builder.Mod = this;
            return builder;
        }

        internal T[] RemoveNulls<T>(T[] data) where T : DataFile
        {
            List<T> list = data.ToList();
            list.RemoveAll(x => x == null || x.ModAdded == this);
            return list.ToArray();
        }

        private T[] DataList<T>(params string[] names) where T : DataFile => names.Select((s) => TryGet<T>(s)).ToArray();


    }

}