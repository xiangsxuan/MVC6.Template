﻿using System;

namespace Renting.Components.Security
{
    public interface IAuthorization
    {
        Boolean IsGrantedFor(Int32? accountId, String area, String controller, String action);

        void Refresh();
    }
}
