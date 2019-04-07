using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JikanDotNet.Tests
{
    public class AnimeTestClass
    {
		private readonly IJikan jikan;

		public AnimeTestClass()
		{
			jikan = new Jikan(true);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(6)]
		public async Task GetAnime_CorrectId_ShouldReturnNotNullAnime(long malId)
        {
			Anime returnedAnime = await jikan.GetAnime(malId);

			Assert.NotNull(returnedAnime);
        }

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		public async Task GetAnime_WrongId_ShouldReturnNullAnime(long malId)
		{
			Anime returnedAnime = await jikan.GetAnime(malId);

			Assert.Null(returnedAnime);
		}

		[Fact]
		public async Task GetAnime_MSGundamId_ShouldParseGundam()
		{
			Anime gundamAnime = await jikan.GetAnime(80);

			Assert.Equal("Mobile Suit Gundam", gundamAnime.Title);
		}

		[Fact]
		public async Task GetAnime_BebopId_ShouldParseCowboyBebop()
		{
			Anime bebopAnime = await jikan.GetAnime(1);

			Assert.Equal("Cowboy Bebop", bebopAnime.Title);
		}

		[Fact]
		public async Task GetAnime_BebopId_ShouldParseCowboyBebopRelatedAnime()
		{
			Anime bebopAnime = await jikan.GetAnime(1);

			Assert.Equal(2, bebopAnime.Related.Adaptations.Count);
			Assert.Equal(2, bebopAnime.Related.SideStories.Count);
			Assert.Single(bebopAnime.Related.Summaries);
		}

		[Fact]
		public async Task GetAnime_FSNId_ShouldParseFateStayNightRelatedAnime()
		{
			Anime fsnAnime = await jikan.GetAnime(356);

			Assert.True(fsnAnime.Related.AlternativeVersions.Count > 3);
		}

		[Fact]
		public async Task GetAnime_FSNReproductionId_ShouldParseFateStayNightReproductionRelatedAnime()
		{
			Anime fsnAnime = await jikan.GetAnime(7559);

			Assert.Equal("Fate/stay night", fsnAnime.Related.FullStories.First().Name);
		}

		[Fact]
		public async Task GetAnime_KamiNomiId_ShouldParseKamiNomiRelatedAnime()
		{
			Anime fsnAnime = await jikan.GetAnime(17725);

			Assert.Equal("Kami nomi zo Shiru Sekai", fsnAnime.Related.ParentStories.First().Name);
		}

		[Fact]
		public async Task GetAnime_CardcaptorId_ShouldParseCardcaptorSakuraInformation()
		{
			Anime cardcaptor = await jikan.GetAnime(232);

			Assert.Equal("70", cardcaptor.Episodes);
			Assert.Equal("TV", cardcaptor.Type);
			Assert.Equal("Spring 1998", cardcaptor.Premiered);
			Assert.Equal("25 min per ep", cardcaptor.Duration);
			Assert.Equal("PG - Children", cardcaptor.Rating);
			Assert.Equal("Tuesdays at 18:00 (JST)", cardcaptor.Broadcast);
			Assert.Equal("Manga", cardcaptor.Source);
		}

		[Fact]
		public async Task GetAnime_AkiraId_ShouldParseAkiraCollections()
		{
			Anime akiraAnime = await jikan.GetAnime(47);

			Assert.Equal(3, akiraAnime.Producers.Count);
			Assert.Equal(3, akiraAnime.Licensors.Count);
			Assert.Equal(1, akiraAnime.Studios.Count);
			Assert.Equal(7, akiraAnime.Genres.Count);
			Assert.Equal("Funimation", akiraAnime.Licensors.First().ToString());
			Assert.Equal("Tokyo Movie Shinsha", akiraAnime.Studios.First().ToString());
			Assert.Equal("Action", akiraAnime.Genres.First().ToString());
		}

		[Fact]
		public async Task GetAnime_KeyTheMetalIdolId_ShouldParseAnimeWithNoRelatedAdaptations()
		{
			Anime returnedAnime = await jikan.GetAnime(1457);

			Assert.Null(returnedAnime.Related.Adaptations);
		}
	}
}
