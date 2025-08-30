//Interface for adding a flight
export interface AddFlight {
  flightNumber: string;
  origin: string;
  destination: string;
  departureTime: Date;
  arrivalTime: Date;
  aircraftId: string;
  status: string;
  priceEconomy: number;
  priceBusiness: number;
}
//Interface for aircraft
export interface Aircraft {
  airCraftId: string;
  model: string;
  totalSeats: number;
  economySeats: number;
  businessSeats: number;
}