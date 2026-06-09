import { SidebarTrigger } from "@/components/ui/sidebar";

type PageHeaderProps = {
  title: string;
  description?: string;
  action?: React.ReactNode;
};

export default function PageHeader({
  title,
  description,
  action,
}: PageHeaderProps) {
  return (
    <header className="bg-background/80 sticky top-0 z-10 flex items-center gap-3 border-b px-4 py-3 backdrop-blur sm:px-6">
      <SidebarTrigger />
      <div className="flex-1">
        <h1 className="text-xl font-bold sm:text-2xl">{title}</h1>
        {description && (
          <p className="text-muted-foreground text-sm">{description}</p>
        )}
      </div>
      {action}
    </header>
  );
}
