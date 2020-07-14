/* Copyright © 2013-2016, Elián Hanisch <lambdae2@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using UnityEngine;

namespace RCSBuildAid
{
    public class CoMMarker : MassEditorMarker
    {
        static CoMMarker instance;

        public static float Mass {
            get { return instance.totalMass; }
        }

        public CoMMarker ()
        {
            instance = this;
        }

        protected override Vector3 UpdatePosition ()
        {
            /* may be required by stock game */
            CraftCoM = base.UpdatePosition ();
            return CraftCoM;
        }

        protected override void calculateCoM (Part part)
        {
            if (part.GroundParts ()) {
                return;
            }

            Vector3 com;
            if (part.GetCoM(out com)) {
                float m = part.GetTotalMass ();
                vectorSum += com * m;
                totalMass += m;
            }
        }
    }
}
