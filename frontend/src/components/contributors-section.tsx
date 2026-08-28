import { ContributorSchema } from "@/domain/schemas/ContributorSchema";
import { Card, CardContent } from "./ui/card";
import { Carousel, CarouselContent, CarouselItem, CarouselNext, CarouselPrevious } from "./ui/carousel";
import Image from "next/image";
type ContributorsSectionProps = {
    contributors: ContributorSchema[]
}
export default function ContributorsSection({ contributors }: ContributorsSectionProps) {

    return (
        <>
            <Carousel
                opts={{
                    align: "start",
                }}
                className="w-full "
            >
                <CarouselContent>
                    {contributors.map((c, index) => (
                        <CarouselItem key={c.id} className="basis-1/2 lg:basis-1/3">
                            <div className="p-1">
                                <Card className="">
                                    <CardContent className="max-h-30 flex items-center justify-center shadow-xl hover:shadow-2xl group rounded-xl transition-all duration-500 transform">
                                        <div>
                                            <div className="flex items-center gap-4">
                                                 {c.profileUrl ? (
                                                    <Image
                                                        src={c.profileUrl}
                                                        alt={c.name}
                                                        width={48}
                                                        height={48}
                                                        unoptimized
                                                        className="w-12 group-hover:w-14 group-hover:h-14 h-12 object-center object-cover rounded-full transition-all duration-500 delay-500 transform"
                                                    />
                                                ) : (
                                                    <div
                                                        aria-hidden
                                                        className="w-12 h-12 flex items-center justify-center rounded-full bg-muted text-sm font-semibold uppercase"
                                                    >
                                                        {c.name.slice(0, 2)}
                                                    </div>
                                                )}
                                                <div className="m-0 w-fit transition-all transform duration-500">
                                                    <h1 className="text-sm truncate font-bold w-full">
                                                        {c.name}
                                                    </h1>
                                                    <p className="text-xs text-gray-400">Contributor</p>
                                                    <p className="text-xs line-clamp-2">
                                                        {c.description}
                                                    </p>
                                                </div>
                                            </div>
                                            <div></div>
                                        </div>
                                        {/* <div
                                            className="relative shadow-xl overflow-hidden hover:shadow-2xl group rounded-xl p-5 transition-all duration-500 transform">
                                            <div className="flex items-center gap-4">
                                                <img src={c.profileUrl}
                                                    className="w-32 group-hover:w-36 group-hover:h-36 h-32 object-center object-cover rounded-full transition-all duration-500 delay-500 transform"
                                                />
                                                <div className="w-fit transition-all transform duration-500">
                                                    <h1 className="text-gray-600 dark:text-gray-200 font-bold">
                                                        {c.name}
                                                    </h1>
                                                    <p className="text-gray-400">Senior Developer</p>
                                                    <a
                                                        className="text-xs text-gray-500 dark:text-gray-200 group-hover:opacity-100 opacity-0 transform transition-all delay-300 duration-500">
                                                        {c.description}
                                                    </a>
                                                </div>
                                            </div>
                                            <div className="absolute group-hover:bottom-1 delay-300 -bottom-16 transition-all duration-500 bg-gray-600 dark:bg-gray-100 right-1 rounded-lg">
                                                <div className="flex justify-evenly items-center gap-2 p-1 text-2xl text-white dark:text-gray-600">
                                                    
                                                </div>
                                            </div>
                                        </div> */}
                                    </CardContent>
                                </Card>
                            </div>
                        </CarouselItem>
                    ))}
                </CarouselContent>
                <CarouselPrevious />
                <CarouselNext />
            </Carousel>
        </>
    )
}