﻿using SimpleDBPersistence.Domain;
using System.Collections.Generic;
using SimpleDBPersistence.SimpleDB.Model;
using System.Text;
using HalalGuide.Domain.Enum;
using System.Linq;

namespace HalalGuide.Domain
{
	[SimpleDBDomain ("Location")]
	public class Location : Entity
	{
		[SimpleDBFieldAttribute ("Name")]
		public string Name { get; set; }

		[SimpleDBFieldAttribute ("AddressRoad")]
		public string AddressRoad { get; set; }

		[SimpleDBFieldAttribute ("AddressPostalCode")]
		public string AddressPostalCode { get; set; }

		[SimpleDBFieldAttribute ("AddressCity")]
		public string AddressCity { get; set; }

		[SimpleDBFieldAttribute ("Latitude")]
		public string Latitude { get; set; }

		[SimpleDBFieldAttribute ("Longtitude")]
		public string Longtitude { get; set; }

		[SimpleDBFieldAttribute ("Telephone")]
		public string Telephone { get; set; }

		[SimpleDBFieldAttribute ("HomePage")]
		public string HomePage { get; set; }

		[SimpleDBFieldAttribute ("LocationType")]
		public LocationType LocationType  { get; set; }

		//Only for Dining
		[SimpleDBListAttribute ("DiningCategory")]
		public List<DiningCategory> Categories  { get; set; }

		//Only for Dining
		[SimpleDBFieldAttribute ("NonHalal")]
		public bool NonHalal { get; set; }

		//Only for Dining
		[SimpleDBFieldAttribute ("Alcohol")]
		public bool Alcohol { get; set; }

		//Only for Dining
		[SimpleDBFieldAttribute ("Pork")]
		public bool Pork { get; set; }

		//Only for Mosque
		[SimpleDBFieldAttribute ("Language")]
		public Language Language { get; set; }

		[SimpleDBFieldAttribute ("LocationStatus")]
		public LocationStatus LocationStatus { get; set; }

		public double Distance { get; set; }

		public Location ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("[Location: Name={0}, Latitude={1}, Longtitude={2}, LocationType={3}, Language={4}, Distance={5}]", Name, Latitude, Longtitude, LocationType, Language, Distance);
		}
		
	}
}

