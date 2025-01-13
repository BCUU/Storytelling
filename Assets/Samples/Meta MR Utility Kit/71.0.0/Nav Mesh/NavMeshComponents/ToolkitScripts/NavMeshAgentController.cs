/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine.AI;

// Navmesh simplifies the job of navigating virtual characters through procedurally created environments.
// By calling 'agent.SetDestination()' and providing a vector3, the agent will discover the best path to the target.
// Object position and rotation will be animated to show movement along a chosen path.

// Additionally, RandomNavPoint can be used for position finding (eg, discover a random floor area in the room for a minigolf hole)

// The interval between changing positions is randomized as well as the characters speed)

public class NavMeshAgentController : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent agent;

    public GameObject girl;

    void OnEnable()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        Vector3 newPos = girl.transform.position;
        var room = MRUK.Instance?.GetCurrentRoom();
        if (!room)
        {
            return;
        }

        if (newPos != null)
        {
            agent.SetDestination(newPos);
        }

    }

}
