﻿/* Copyright © 2013-2016, Elián Hanisch <lambdae2@gmail.com>
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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace RCSBuildAid
{
    public static class EditorUtils
    {
        static List<PartModule> tempList;
        static Type partModuleType;

        public static bool isInputFieldFocused ()
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (obj == null) {
                return false;
            }
            TMP_InputField input = obj.GetComponent<TMP_InputField> ();
            if (input == null) {
                return false;
            }
            return input.isFocused;
        }

        public static List<PartModule> GetModulesOf<T> () where T : PartModule
        {
            tempList = new List<PartModule> ();
            partModuleType = typeof(T);
            RunOnVesselParts (findModules);
            return tempList;
        }
        
        public static List<PartModule> GetSelectedModulesOf<T> () where T : PartModule
        {
            tempList = new List<PartModule> ();
            partModuleType = typeof(T);
            RunOnSelectedParts(findModules);
            return tempList;
        }

        static void findModules (Part part)
        {
            /* check if this part has a module of type T */
            for (int i = part.Modules.Count - 1; i >= 0; i--) {
                var mod = part.Modules [i];
                var modType = mod.GetType ();
                if ((modType == partModuleType) || modType.IsSubclassOf (partModuleType)) {
                    tempList.Add (mod);
                }
            }
        }

        public static void RunOnVesselParts(Action<Part> f)
        {
            if (EditorLogic.RootPart == null) {
                return;
            }
            /* run in vessel's parts */
            var parts = EditorLogic.fetch.ship.parts;
            for (int i = 0; i < parts.Count; i++) {
                f (parts[i]);
            }
        }

        public static void RunOnSelectedParts(Action<Part> f)
        {
            /* run in selected parts that are connected */
            if (EditorLogic.SelectedPart != null) {
                Part part = EditorLogic.SelectedPart;
                // TODO is the EditorLogic check needed?
                if (!EditorLogic.fetch.ship.Contains (part) && (part.potentialParent != null)) {
                    recursePart (part, f);
                    for (int i = 0; i < part.symmetryCounterparts.Count; i++) {
                        recursePart(part.symmetryCounterparts [i], f);
                    }
                }
            }
        }

        static void recursePart (Part part, Action<Part> f)
        {
            f (part);
            for (int i = 0; i < part.children.Count; i++) {
                recursePart (part.children [i], f);
            }
        }
    }
}

