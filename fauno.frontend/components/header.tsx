"use client"

import React, { useState } from 'react'
import { BellIcon, BellRingingIcon } from '@phosphor-icons/react'

type HeaderProps = {
    title: string;
}

export default function Header({ title }: HeaderProps) {
    const [isOpen, setIsOpen] = useState(false);

    const toggleNotification = () => {
        setIsOpen(!isOpen);
    }

    return (
        <div className="flex w-full justify-between items-center gap-4 p-5 px-6">
            <h1 className="text-3xl font-bold">{title}</h1>
            <BellRingingIcon size={24} onClick={toggleNotification} />
        </div>
    )
}
