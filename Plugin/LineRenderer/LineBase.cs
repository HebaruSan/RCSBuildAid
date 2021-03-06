/* Copyright © 2013-2020, Elián Hanisch <lambdae2@gmail.com>
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

using System.Collections.Generic;
using UnityEngine;

namespace RCSBuildAid
{
    public class LineBase : MonoBehaviour
    {
        const string shader = "Legacy Shaders/Particles/Alpha Blended";

        /* Need SerializeField or cloning will fail to pick these private variables */
        [SerializeField]
        protected Color color = Color.cyan;
        [SerializeField]
        protected List<LineRenderer> lines = new List<LineRenderer> ();
        
        Material material;

        const int layer = 2;

        public virtual void setColor (Color value) {
            color = value;
            foreach(var line in lines) {
                line.startColor = value;
                line.endColor = value;
            }
        }

        public virtual void setWidth(float v1, float v2) {
            foreach(var line in lines) {
                line.startWidth = v1;
                line.endWidth = v2;
            }
        }

        public virtual void setWidth (float v2)
        {
            setWidth (v2, v2);
        }

        protected LineRenderer newLine ()
        {
#if DEBUG
            Debug.Log("[RCSBA, LineBase]: new line.");
#endif
            var obj = new GameObject("RCSBuildAid LineRenderer GameObject");
            var lr = obj.AddComponent<LineRenderer>();
            obj.transform.parent = gameObject.transform;
            obj.transform.localPosition = Vector3.zero;
            lr.material = material;
            lines.Add(lr);
            return lr;
        }

        protected virtual void Awake ()
        {
            material = new Material (Shader.Find (shader));
        }

        protected virtual void Start ()
        {
            setColor (color);
            setLayer ();
        }

        protected virtual void LateUpdate ()
        {
            checkLayer ();
        }

        void checkLayer ()
        {
            /* the Editor clobbers the layer's value whenever you pick the part */
            if (gameObject.layer != layer) {
                setLayer ();
            }
        }

        public virtual void setLayer ()
        {
            gameObject.layer = layer;
            foreach(var line in lines) {
                line.gameObject.layer = layer;
            }
        }
    }
}

