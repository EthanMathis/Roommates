select rm.id, rm.RentPortion, rm.FirstName, rm.LastName, rm.MoveInDate, rm.RoomId, r.Name as RoomName, r.MaxOccupancy 
                    FROM Roommate rm LEFT JOIN Room r ON r.Id = rm.RoomId