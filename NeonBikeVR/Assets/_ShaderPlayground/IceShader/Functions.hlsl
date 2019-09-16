#ifndef MYFUNCTIONINCLUDE_INCLUDED
#define MYFUNCTIONINCLUDE_INCLUDED

void parallax_float(float2 UV, float3 TangentViewDir, float heightValue, out float2 ParallaxUV)
{
    //Where is the ray starting? y is up and we always start at the surface
    float3 rayPos = float3(UV.x, 0, UV.y);

    //What's the direction of the ray?
    float3 rayDir = normalize(TangentViewDir);

    //Find where the ray is intersecting with the terrain with a raymarch algorithm
    int STEPS = 300;
    float stepDistance = 0.01;

    ParallaxUV = UV;

    for (int i = 0; i < STEPS; i++)
    {
        //Get the current height at this uv coordinate
        float height = heightValue;

        //If the ray is below the surface
        if (rayPos.y < height)
        {
            //Move one step back to the position before we hit terrain
            float3 oldPos = rayPos - stepDistance * rayDir;

            float oldHeight = heightValue;

            //Always positive
            float oldDistToTerrain = abs(oldHeight - oldPos.y);

            float currentHeight = heightValue;

            //Always negative
            float currentDistToTerrain = rayPos.y - currentHeight;

            float weight = currentDistToTerrain / (currentDistToTerrain - oldDistToTerrain);

            //Calculate a weighted texture coordinate
            //If height is -2 and oldHeight is 2, then weightedTex is 0.5, which is good because we should use 
            //the exact middle between the coordinates
            float2 weightedTexPos = oldPos.xz * weight + rayPos.xz * (1 - weight);

            //Get the texture position by interpolation between the position where we hit terrain and the position before
            float2 weightedTex = weightedTexPos; 
            
            float height = heightValue;
            
            ParallaxUV = weightedTex;
            
            //We have hit the terrain so we dont need to loop anymore	
            break;
        }
        
        //Move along the ray
        rayPos += stepDistance * rayDir;
    }
}

#endif