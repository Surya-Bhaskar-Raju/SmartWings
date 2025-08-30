export default interface SearchFlight {
  flightId: string;
  flightNumber: string;
  origin: string;
  destination: string;
  departureTime: Date;
  arrivalTime: Date;
  airCraftId:string;
  status: string;
  priceEconomy: number;
  priceBusiness: number;
}
