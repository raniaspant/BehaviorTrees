using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using BTnamespace;

/* @class Tagged
 * @brief In this class, success result is given if the bot is tagged, failure message if otherwise.
 *	Hint: makes more sense as a class if combined with some further interaction from the teammates.
 *	For example, one of my first intentions was to make the teammate "dead" and not "tagged", therefore
 *	its teammates would have to decide who will go and resurrect him, in order to keep the team numbers even and fair.
 */

namespace CTF
{
    class Tagged : Leaf
    {
        public override void Initialize(BehaviorTree tree)
        {

        }
        public override Outcome Process(BehaviorTree tree)
        {
            bool isTagged = (bool)tree.getValue(TreeData.IS_TAGGED);
            if (true == isTagged)
                return Outcome.SUCCESS;
            return Outcome.FAIL;
        }
    }
}
