﻿using ConcertTicketAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcertTicketAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid?> ReserveTicketsAsync(Guid ticketTypeId, int quantity, Guid userId, TimeSpan reservationDuration)
        {
            var ticketType = await _context.TicketTypes.Include(t => t.Event)
                .FirstOrDefaultAsync(t => t.Id == ticketTypeId);

            if (ticketType == null || ticketType.QuantityAvailable < quantity)
                return null;

            var reservationId = Guid.NewGuid();
            var reservedUntil = DateTime.UtcNow.Add(reservationDuration);

            for (int i = 0; i < quantity; i++)
            {
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    TicketTypeId = ticketTypeId,
                    UserId = userId,
                    Status = TicketStatus.Reserved,
                    ReservedUntil = reservedUntil,
                    ReservationId = reservationId
                };
                _context.Tickets.Add(ticket);
            }

            ticketType.QuantityAvailable -= quantity;
            await _context.SaveChangesAsync();
            return reservationId;
        }

        public async Task<bool> PurchaseTicketsAsync(Guid reservationId, Guid userId)
        {
            var reservation = await _context.Tickets
                .Where(t => t.ReservationId == reservationId && t.Status == TicketStatus.Reserved)
                .FirstOrDefaultAsync();

            if (reservation == null)
                return false;

            reservation.Status = TicketStatus.Purchased;
            reservation.UserId = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ticket>> GetUserPurchasedTicketsAsync(Guid userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId && t.Status == TicketStatus.Purchased)
                .Include(t => t.TicketType)
                .ThenInclude(tt => tt.Event)
                .ToListAsync();
        }

        public async Task<bool> CancelReservationAsync(Guid reservationId, Guid userId)
        {
            var reservation = await _context.Tickets
                .FirstOrDefaultAsync(t => t.ReservationId == reservationId && t.Status == TicketStatus.Reserved && t.UserId == userId);

            if (reservation == null)
                return false; 

            reservation.Status = TicketStatus.Cancelled;
            reservation.UserId = null;

            var ticketType = await _context.TicketTypes.FindAsync(reservation.TicketTypeId);
            if (ticketType != null)
            {
                ticketType.QuantityAvailable += 1;
            }

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
