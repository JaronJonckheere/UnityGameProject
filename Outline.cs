change the logic to this

if (distance from player to chest is < 0.75 and mouse is on top of chest) 
    {
    if (mouse clicked) 
    {
        if (chest is in closed state)
        {
            open chest animation
        }
        if (chest is in open state)
        {
            nothing (stay in open state)
        }
    }
}
else if (distance from player to chest is >= 0.75 or ESC is pressed)
    { 
    if (chest is in open state)
    {
        close chest animation
    }
    else if (chest is in closed state)
    { 
        nothing (stay in closed state)
    }
}